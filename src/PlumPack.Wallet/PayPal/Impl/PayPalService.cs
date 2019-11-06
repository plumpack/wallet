using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Orders;
using PlumPack.Infrastructure;
using PlumPack.Wallet.Accounts;
using PlumPack.Wallet.Domain;
using PlumPack.Wallet.Transactions;
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
        private readonly ITransactionsService _transactionsService;

        public PayPalService(IPayPalClientProvider payPalClientProvider,
            IOptions<PayPalOptions> options,
            ILogger<PayPalService> logger,
            IAccountsService accountsService,
            IDataService dataService,
            ITransactionsService transactionsService)
        {
            _payPalClientProvider = payPalClientProvider;
            _options = options;
            _logger = logger;
            _accountsService = accountsService;
            _dataService = dataService;
            _transactionsService = transactionsService;
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
                    _logger.LogError("Invalid status code from PayPal order create: status: {Status} response: {Response}", response.StatusCode, SerializePayPalType(response.Result<object>()));
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

        public async Task CapturePendingOrder(string orderId, string payerId)
        {
            Order order;
            {
                var client = _payPalClientProvider.BuildHttpClient();
                var request = new OrdersCaptureRequest(orderId);
                request.RequestBody(new OrderActionRequest());
                var response = await client.Execute(request);

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    _logger.LogError("Invalid status code from PayPal order capture: status: {Status} response: {Response}", response.StatusCode, SerializePayPalType(response.Result<object>()));
                    throw new Exception("Invalid PayPal response");
                }

                order = response.Result<Order>();

                if (order.Status != "COMPLETED")
                {
                    _logger.LogError("The order from PayPal wasn't marked as COMPELTED: {Response}", SerializePayPalType(response.Result<object>()));
                    throw new Exception("The order from PayPal wasn't marked as COMPELTED");
                }
            }

            using (var con = new ConScope(_dataService))
            using (var trans = await con.BeginTransaction())
            {
                var pendingOrder = con.Connection.Single<PendingPayPalOrder>(x => x.PayPalOrderId == orderId);
                pendingOrder.PayPalPayerId = payerId;
                pendingOrder.PayPalOrderJson = SerializePayPalType(order);
                
                await con.Connection.SaveAsync(pendingOrder);

                await _transactionsService.AddTransaction(pendingOrder.AccountId, pendingOrder.Amount, "PayPal reload", pendingOrder.PayPalOrderJson);
                
                trans.Commit();
            }
        }

        private string SerializePayPalType(object value)
        {
            value.NotNull();
            
            using (var memoryStream = new MemoryStream())
            {
                new DataContractJsonSerializer(value.GetType()).WriteObject(memoryStream, value);
                memoryStream.Position = 0;
                using (var streamReader = new StreamReader(memoryStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}