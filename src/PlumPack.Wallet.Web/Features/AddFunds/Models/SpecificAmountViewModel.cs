namespace PlumPack.Wallet.Web.Features.AddFunds.Models
{
    public class SpecifyAmountViewModel : SpecifyAmountInputModel
    {
        
    }
    
    public class SpecifyAmountInputModel
    {
        public SpecifyAmountInputModel()
        {
            Amount = 25.00m;
        }
        
        public decimal Amount { get; set; }
    }
}