using PayPalCheckoutSdk.Core;
using PlumPack.Infrastructure;

namespace PlumPack.Wallet.PayPal.Impl
{
    [Service(typeof(IPayPalClientProvider))]
    public class PayPalClientProvider : IPayPalClientProvider
    {
        private readonly IPayPalEnvironmentProvider _payPalEnvironmentProvider;

        public PayPalClientProvider(IPayPalEnvironmentProvider payPalEnvironmentProvider)
        {
            _payPalEnvironmentProvider = payPalEnvironmentProvider;
        }
        
        public PayPalHttpClient BuildHttpClient()
        {
            return new PayPalHttpClient(_payPalEnvironmentProvider.BuildEnvironment());
        }
    }
}