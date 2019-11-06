using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlumPack.IdentityServer.Client;
using PlumPack.Wallet.Accounts;
using PlumPack.Wallet.PayPal;
using PlumPack.Wallet.Web.Features.PayPal.Models;
using PlumPack.Web;

namespace PlumPack.Wallet.Web.Features.PayPal
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PayPalController : BaseController
    {
        private readonly IPayPalService _payPalService;
        private readonly IAccountsService _accountsService;

        public PayPalController(IPayPalService payPalService,
            IAccountsService accountsService)
        {
            _payPalService = payPalService;
            _accountsService = accountsService;
        }
        
        [HttpPost]
        [Route("create-order")]
        public async Task<CreateOrderResponse> CreateOrder([FromBody]CreateOrderRequest request)
        {
            var account = await _accountsService.GetOrCreateAccount(AccountIdentification.ByGlobalUserId(User.GetUserId()));
            
            var pendingOrder = await _payPalService.CreatePendingOrder(account.Id, request.Amount);
            
            return new CreateOrderResponse
            {
                OrderId = pendingOrder.PayPalOrderId
            };
        }

        [HttpPost]
        [Route("capture-order")]
        public async Task<CaptureOrderResponse> CaptureOrder([FromBody]CaptureOrderRequest request)
        {
            await _payPalService.CapturePendingOrder(request.OrderId, request.PayerId);
            return new CaptureOrderResponse();
        }
    }
}