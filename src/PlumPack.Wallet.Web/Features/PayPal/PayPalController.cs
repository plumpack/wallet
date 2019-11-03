using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlumPack.Wallet.PayPal;
using PlumPack.Web;

namespace PlumPack.Wallet.Web.Features.PayPal
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PayPalController : BaseController
    {
        private readonly IPayPalService _payPalService;

        public PayPalController(IPayPalService payPalService)
        {
            _payPalService = payPalService;
        }
        
        public async Task<PayPalCheckoutSdk.Orders.Order> CreateTransaction()
        {
            var response = await _payPalService.CreateTransaction();
            return response;
        }
    }
}