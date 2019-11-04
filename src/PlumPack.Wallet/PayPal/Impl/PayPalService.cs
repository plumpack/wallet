using System;
using System.Collections.Generic;
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
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            Value = "23.00",
                            CurrencyCode = "USD"
                        }
                    }
                }
            });
            
//            ApplicationContext = new ApplicationContext
//  {
//    BrandName = "EXAMPLE INC",
//    LandingPage = "BILLING",
//    UserAction = "CONTINUE",
//    ShippingPreference = "SET_PROVIDED_ADDRESS"
//  },
//  PurchaseUnits = new List<PurchaseUnitRequest>
//  {
//    new PurchaseUnitRequest{
//      ReferenceId =  "PUHF",
//      Description = "Sporting Goods",
//      CustomId = "CUST-HighFashions",
//      SoftDescriptor = "HighFashions",
//      Amount = new AmountWithBreakdown
//      {
//        CurrencyCode = "USD",
//        Value = "230.00",
//        Breakdown = new AmountBreakdown
//        {
//          ItemTotal = new Money
//          {
//            CurrencyCode = "USD",
//            Value = "180.00"
//          },
//          Shipping = new Money
//          {
//            CurrencyCode = "USD",
//            Value = "30.00"
//          },
//          Handling = new Money
//          {
//            CurrencyCode = "USD",
//            Value = "10.00"
//          },
//          TaxTotal = new Money
//          {
//            CurrencyCode = "USD",
//            Value = "20.00"
//          },
//          ShippingDiscount = new Money
//          {
//            CurrencyCode = "USD",
//            Value = "10.00"
//          }
//        }
//      },
//      Items = new List<Item>
//      {
//        new Item
//        {
//          Name = "T-shirt",
//          Description = "Green XL",
//          Sku = "sku01",
//          UnitAmount = new Money
//          {
//            CurrencyCode = "USD",
//            Value = "90.00"
//          },
//          Tax = new Money
//          {
//            CurrencyCode = "USD",
//            Value = "10.00"
//          },
//          Quantity = "1",
//          Category = "PHYSICAL_GOODS"
//        },
//        new Item
//        {
//          Name = "Shoes",
//          Description = "Running, Size 10.5",
//          Sku = "sku02",
//          UnitAmount = new Money
//          {
//            CurrencyCode = "USD",
//            Value = "45.00"
//          },
//          Tax = new Money
//          {
//            CurrencyCode = "USD",
//            Value = "5.00"
//          },
//          Quantity = "2",
//          Category = "PHYSICAL_GOODS"
//        }
//      },
//      Shipping = new ShippingDetails
//      {
//        Name = new Name
//        {
//          FullName = "John Doe"
//        },
//        AddressPortable = new AddressPortable
//        {
//          AddressLine1 = "123 Townsend St",
//          AddressLine2 = "Floor 6",
//          AdminArea2 = "San Francisco",
//          AdminArea1 = "CA",
//          PostalCode = "94107",
//          CountryCode = "US"
//        }
//      }
//    }
//  }
            
            var response = await client.Execute(request);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                _logger.LogError("Invalid status code from PayPal: status: {@Status} response: {@Response}", response.StatusCode, response.Result<object>());
                throw new Exception("Invalid PayPal response");
            }

            return response.Result<Order>();
        }
    }
}