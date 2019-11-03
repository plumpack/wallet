using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PlumPack.Infrastructure;

namespace PlumPack.Wallet.PayPal.Impl
{
    [Service(typeof(IPayPalService))]
    public class PayPalService : IPayPalService
    {
        private readonly IPayPalClientProvider _payPalClientProvider;
        private readonly IOptions<PayPalOptions> _options;
        private readonly ILogger<PayPalService> _logger;

        public PayPalService(IPayPalClientProvider payPalClientProvider, IOptions<PayPalOptions> options,
            ILogger<PayPalService> logger)
        {
            _payPalClientProvider = payPalClientProvider;
            _options = options;
            _logger = logger;
        }

        public string ClientId => _options.Value.ClientId;

        public async Task<Order> CreateTransaction()
        {
            var client = _payPalClientProvider.BuildHttpClient();
            
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(new OrderRequest
            {
            });
            
            var response = await client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError("Invalid status code from PayPal: status: {@Status} response: {@Response}", response.StatusCode, response.Result<object>());
                throw new Exception("Invalid PayPal response");
            }

            return response.Result<Order>();
        }
    }
}