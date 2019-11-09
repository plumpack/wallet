namespace PlumPack.Wallet.Web.Features.AddFunds.Models
{
    public class CancelPayPalOrderRequest
    {
        public string OrderId { get; set; }
        
        public string FlowId { get; set; }
    }

    public class CancelPayPalOrderResponse
    {
        
    }
}