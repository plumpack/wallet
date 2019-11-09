namespace PlumPack.Wallet.Web.Features.AddFunds.Models
{
    public class FundViewModel
    {
        public string PayPalClientId { get; set; }
        
        public string FlowId { get; set; }
        
        public decimal Amount { get; set; }
    }
}