using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public PayPalController(IPayPalService payPalService)
        {
            _payPalService = payPalService;
        }
        
        [HttpPost]
        [Route("create-transaction")]
        public async Task<CreateTransactionResponse> CreateTransaction(CreateTransactionRequest request)
        {
            var transaction = await _payPalService.CreateTransaction();
            return new CreateTransactionResponse
            {
                OrderId = transaction.Id
            };
        }
    }
}