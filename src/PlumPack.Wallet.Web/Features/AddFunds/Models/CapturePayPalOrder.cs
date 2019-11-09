namespace PlumPack.Wallet.Web.Features.AddFunds.Models
{
    public class CapturePayPalOrderRequest
    {
        public string OrderId { get; set; }
        
        public string PayerId { get; set; }
        
        public string FlowId { get; set; }
    }

    public class CapturePayPalOrderResponse
    {
        
    }
}