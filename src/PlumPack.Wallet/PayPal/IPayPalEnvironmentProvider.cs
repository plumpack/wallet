using PayPalCheckoutSdk.Core;

namespace PlumPack.Wallet.PayPal
{
    public interface IPayPalEnvironmentProvider
    {
        PayPalEnvironment BuildEnvironment();
    }
}