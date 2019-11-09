using System;
using ServiceStack.DataAnnotations;

namespace PlumPack.Wallet.Domain
{
    [Alias("accounts")]
    public class Account
    {
        [Alias("id"), PrimaryKey, AutoId, Required]
        public Guid Id { get; set; }
        
        [Alias("global_user_id"), Required]
        public Guid GlobalUserId { get; set; }
        
        [Alias("current_balance"), Required]
        public decimal CurrentBalance { get; set; }
    }
}