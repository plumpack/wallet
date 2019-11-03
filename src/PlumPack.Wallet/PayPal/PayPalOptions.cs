namespace PlumPack.Wallet.PayPal
{
    /// <summary>
    /// For dev sandbox: https://developer.paypal.com
    /// </summary>
    public class PayPalOptions
    {
        public string Account { get; set; }
        
        public string ClientId { get; set; }
        
        public string Secret { get; set; }
        
        public bool Sandbox { get; set; }
    }
}