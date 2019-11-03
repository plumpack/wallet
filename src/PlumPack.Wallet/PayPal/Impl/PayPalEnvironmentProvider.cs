using PayPalCheckoutSdk.Core;
using PlumPack.Infrastructure;

namespace PlumPack.Wallet.PayPal.Impl
{
    [Service(typeof(IPayPalEnvironmentProvider))]
    public class PayPalEnvironmentProvider : IPayPalEnvironmentProvider
    {
        private readonly Microsoft.Extensions.Options.IOptions<PayPalOptions> _options;

        public PayPalEnvironmentProvider(Microsoft.Extensions.Options.IOptions<PayPalOptions> options)
        {
            _options = options;
        }
        
        public PayPalEnvironment BuildEnvironment()
        {
            var options = _options.Value;
            options.Account.NotNullOrEmpty(nameof(PayPalOptions.Account));
            options.ClientId.NotNullOrEmpty(nameof(PayPalOptions.ClientId));
            options.Secret.NotNullOrEmpty(nameof(PayPalOptions.Secret));

            if (options.Sandbox)
            {
                return new SandboxEnvironment(options.ClientId, options.Secret);
            }

            return new LiveEnvironment(options.ClientId, options.Secret);
        }
    }
}