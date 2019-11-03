using ServiceStack.DataAnnotations;

namespace PlumPack.Wallet.Domain
{
    [Alias("transactions")]
    public class Transaction
    {
        [Alias("id"), PrimaryKey, Required]
        public string Id { get; set; }
        
        [Alias("amount")]
        public decimal Amount { get; set; }
    }
}