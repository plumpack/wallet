using PlumPack.Web;

namespace PlumPack.Wallet.Web.Features.PayPal.Models
{
    public class CreateTransactionRequest
    {
        public decimal Amount { get; set; }
    }

    public class CreateTransactionResponse : BaseResponse
    {
        public string OrderId { get; set; }
    }
}