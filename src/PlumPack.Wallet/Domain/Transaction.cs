using System;
using ServiceStack.DataAnnotations;

namespace PlumPack.Wallet.Domain
{
    [Alias("transactions")]
    public class Transaction
    {
        [Alias("id"), PrimaryKey, AutoId, Required]
        public Guid Id { get; set; }
        
        [Alias("account_id"), Required]
        [References(typeof(Account))]
        public Guid AccountId { get; set; }
        
        [Alias("amount"), Required]
        public decimal Amount { get; set; }
        
        [Alias("title"), Required]
        public string Title { get; set; }
        
        [Alias("meta_data")]
        public string MetaData { get; set; }
    }
}