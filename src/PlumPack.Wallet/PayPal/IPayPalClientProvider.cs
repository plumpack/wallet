using PayPalCheckoutSdk.Core;

namespace PlumPack.Wallet.PayPal
{
    public interface IPayPalClientProvider
    {
        PayPalHttpClient BuildHttpClient();
    }
}