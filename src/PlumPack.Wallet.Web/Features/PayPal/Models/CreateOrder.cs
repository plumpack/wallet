using PlumPack.Web;

namespace PlumPack.Wallet.Web.Features.PayPal.Models
{
    public class CreateOrderRequest
    {
        public decimal Amount { get; set; }
    }

    public class CreateOrderResponse : BaseResponse
    {
        public string OrderId { get; set; }
    }
}