using PlumPack.Web;

namespace PlumPack.Wallet.Web.Features.PayPal.Models
{
    public class CaptureOrderRequest
    {
        public string OrderId { get; set; }
        
        public string PayerId { get; set; }
    }

    public class CaptureOrderResponse : BaseResponse
    {
        
    }
}