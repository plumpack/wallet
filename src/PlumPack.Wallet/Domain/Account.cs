using ServiceStack.DataAnnotations;

namespace PlumPack.Wallet.Domain
{
    [Alias("accounts")]
    public class Account
    {
        [Alias("id"), PrimaryKey, Required]
        public string Id { get; set; }
    }
}