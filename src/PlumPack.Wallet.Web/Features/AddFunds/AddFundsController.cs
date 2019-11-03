using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlumPack.Wallet.PayPal;
using PlumPack.Wallet.Web.Features.AddFunds.Models;
using PlumPack.Web;

namespace PlumPack.Wallet.Web.Features.AddFunds
{
    [Authorize]
    public class AddFundsController : BaseController
    {
        private readonly IPayPalService _payPalService;

        public AddFundsController(IPayPalService payPalService)
        {
            _payPalService = payPalService;
        }
        
        public ActionResult Index()
        {
            var vm = new AddFundsViewModel();
            vm.PayPalClientId = _payPalService.ClientId;
            return View(vm);
        }   
    }
}