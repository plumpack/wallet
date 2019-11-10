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
        
        [Alias("date"), Required]
        public DateTimeOffset Date { get; set; }
        
        [Alias("amount"), Required]
        public decimal Amount { get; set; }
        
        [Alias("title"), Required]
        public string Title { get; set; }
        
        [Alias("meta_data")]
        public string MetaData { get; set; }
        
        [Alias("paypal_order_id")]
        [References(typeof(PayPalOrder))]
        public Guid? PayPalOrderId { get; set; }
    }
}