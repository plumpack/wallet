using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PlumPack.Infrastructure;
using PlumPack.Wallet.Accounts;
using PlumPack.Wallet.Domain;
using ServiceStack.OrmLite;
using SharpDataAccess.Data;

namespace PlumPack.Wallet.PayPal.Impl
{
    [Service(typeof(IPayPalService))]
    public class PayPalService : IPayPalService
    {
        private readonly IPayPalClientProvider _payPalClientProvider;
        private readonly IOptions<PayPalOptions> _options;
        private readonly ILogger<PayPalService> _logger;
        private readonly IAccountsService _accountsService;
        private readonly IDataService _dataService;

        public PayPalService(IPayPalClientProvider payPalClientProvider,
            IOptions<PayPalOptions> options,
            ILogger<PayPalService> logger,
            IAccountsService accountsService,
            IDataService dataService)
        {
            _payPalClientProvider = payPalClientProvider;
            _options = options;
            _logger = logger;
            _accountsService = accountsService;
            _dataService = dataService;
        }

        public string ClientId => _options.Value.ClientId;


        public async Task<PendingPayPalOrder> CreatePendingOrder(Guid accountId, decimal amount)
        {
            // TODO: Make this range configurable.
            if (amount <= 0 || amount > 500)
            {
                throw new Exception("Invalid amount.");
            }
         
            _logger.LogInformation("Creating pending paypal order for {AccountId} for {Amount}", accountId, amount);
            
            var account = await _accountsService.GetAccount(AccountIdentification.ById(accountId));
            account.NotNull();

            var potentialOrderId = Guid.NewGuid();
            string paypalOrderId;
            
            {
                var client = _payPalClientProvider.BuildHttpClient();
                var request = new OrdersCreateRequest();
                request.Prefer("return=representation");
                request.RequestBody(new OrderRequest
                {
                    CheckoutPaymentIntent = "CAPTURE",
                    PurchaseUnits = new List<PurchaseUnitRequest>
                    {
                        new PurchaseUnitRequest
                        {
                            ReferenceId = potentialOrderId.ToString(),
                            AmountWithBreakdown = new AmountWithBreakdown
                            {
                                Value = amount.ToString(CultureInfo.InvariantCulture),
                                CurrencyCode = "USD"
                            }
                        }
                    }
                });
                
                var response = await client.Execute(request);

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    _logger.LogError("Invalid status code from PayPal: status: {@Status} response: {@Response}", response.StatusCode, response.Result<object>());
                    throw new Exception("Invalid PayPal response");
                }

                paypalOrderId = response.Result<Order>().Id;
            }
            
            // Great, we created a PayPal order, let's add the record into the database.
            using (var con = new ConScope(_dataService))
            {
                var pendingOrder = new PendingPayPalOrder
                {
                    Id = potentialOrderId,
                    PayPalOrderId = paypalOrderId,
                    AccountId = account.Id,
                    Amount = amount
                };
                await con.Connection.InsertAsync(pendingOrder);

                return pendingOrder;
            }
        }
    }
}