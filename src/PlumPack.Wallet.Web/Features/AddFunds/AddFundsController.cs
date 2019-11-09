using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PlumPack.IdentityServer.Client;
using PlumPack.Infrastructure;
using PlumPack.Wallet.Accounts;
using PlumPack.Wallet.PayPal;
using PlumPack.Wallet.Web.Features.AddFunds.Models;
using PlumPack.Web;

namespace PlumPack.Wallet.Web.Features.AddFunds
{
    [Authorize]
    public class AddFundsController : BaseController
    {
        private readonly IPayPalService _payPalService;
        private readonly IDistributedCache _distributedCache;
        private readonly IAccountsService _accountsService;

        public AddFundsController(IPayPalService payPalService,
            IDistributedCache distributedCache,
            IAccountsService accountsService)
        {
            _payPalService = payPalService;
            _distributedCache = distributedCache;
            _accountsService = accountsService;
        }
        
        public ActionResult Index()
        {
            var vm = new SpecifyAmountViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(SpecifyAmountInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new SpecifyAmountViewModel
                {
                    Amount = model.Amount
                });
            }

            var flowId = Guid.NewGuid().ToString();
            await _distributedCache.SetAsync($"flow-{flowId}", JsonSerializer.SerializeToUtf8Bytes(new Flow
            {
                Amount = model.Amount,
                AccountId = (await _accountsService.GetOrCreateAccount(AccountIdentification.ByGlobalUserId(User.GetUserId()))).Id
            }), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            return RedirectToAction("Fund", new {flowId = flowId});
        }

        public async Task<ActionResult> Fund(string flowId)
        {
            var (flow, _) = await GetFlow(flowId);

            var vm = new FundViewModel
            {
                PayPalClientId = _payPalService.ClientId,
                FlowId = flowId,
                Amount = flow.Amount
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> PayPalOrderCreate([FromBody]CreatePayPalOrderRequest request)
        {
            var (flow, _) = await GetFlow(request.FlowId);

            var account = await _accountsService.GetOrCreateAccount(AccountIdentification.ByGlobalUserId(User.GetUserId()));
            
            var pendingOrder = await _payPalService.CreatePendingOrder(account.Id, flow.Amount);

            return Json(new CreatePayPalOrderResponse
            {
                OrderId = pendingOrder.PayPalOrderId
            });
        }
        
        [HttpPost]
        public async Task<ActionResult> PayPalOrderCapture([FromBody]CapturePayPalOrderRequest request)
        {
            // We are only calling this to validate the flow id, and that it is for the current authorized user/account.
            await GetFlow(request.FlowId);
            
            await _payPalService.CapturePendingOrder(request.OrderId, request.PayerId);
            
            return Json(new CapturePayPalOrderResponse());
        }

        [HttpPost]
        public async Task<ActionResult> PayPalOrderCancel([FromBody]CancelPayPalOrderRequest request)
        {
            // We are only calling this to validate the flow id, and that it is for the current authorized user/account.
            await GetFlow(request.FlowId);

            await _payPalService.CancelPendingOrder(request.OrderId);

            return Json(new CancelPayPalOrderResponse());
        }

        public async Task<ActionResult> PayPalOrderCompleted(string flowId)
        {
            // We are only calling this to validate the flow id, and that it is for the current authorized user/account.
            await GetFlow(flowId);
            
            _distributedCache.Remove($"flow-{flowId}");

            AddSuccessMessage("Your funds were successfully loaded.", true);
            return RedirectToAction("Index", "Home");
        }

        private async Task<(Flow flow, Domain.Account)> GetFlow(string flowId)
        {
            flowId.NotNullOrEmpty();
            
            Flow flow = null;
            {
                var data = await _distributedCache.GetAsync($"flow-{flowId}");
                if (data == null || data.Length == 0)
                {
                    throw new Exception($"Invalid flow {flowId}");
                }

                flow = JsonSerializer.Deserialize<Flow>(data);
            }

            flow.NotNull();
            
            var account = await _accountsService.GetOrCreateAccount(AccountIdentification.ByGlobalUserId(User.GetUserId()));
            if (flow.AccountId != account.Id)
            {
                throw new Exception("Flow for wrong account.");
            }

            return (flow, account);
        }
        
        private class Flow
        {
            public decimal Amount { get; set; }
            
            public Guid AccountId { get; set; }
        }
    }
}