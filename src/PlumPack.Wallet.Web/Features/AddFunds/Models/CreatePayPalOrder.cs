using PlumPack.Web;

namespace PlumPack.Wallet.Web.Features.AddFunds.Models
{
    public class CreatePayPalOrderRequest
    {
        public string FlowId { get; set; }
    }

    public class CreatePayPalOrderResponse
    {
        public string OrderId { get; set; }
    }
}