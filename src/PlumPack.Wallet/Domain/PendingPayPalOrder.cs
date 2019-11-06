using System;
using ServiceStack.DataAnnotations;

namespace PlumPack.Wallet.Domain
{
    [Alias("pending_paypal_order")]
    public class PendingPayPalOrder
    {
        [Alias("id"), PrimaryKey, AutoId, Required]
        public Guid Id { get; set; }
        
        [Alias("paypal_order_id"), Required]
        public string PayPalOrderId { get; set; }
        
        [Alias("paypal_payer_id")]
        public string PayPalPayerId { get; set; }
        
        [Alias("paypal_order_json")]
        public string PayPalOrderJson { get; set; }
        
        [Alias("account_id"), Required]
        [References(typeof(Account))]
        public Guid AccountId { get; set; }
        
        [Alias("amount"), Required]
        public decimal Amount { get; set; }
    }
}