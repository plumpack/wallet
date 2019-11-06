using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PayPalCheckoutSdk.Orders;
using PlumPack.Wallet.Domain;

namespace PlumPack.Wallet.PayPal
{
    public interface IPayPalService
    {
        string ClientId { get; }

        Task<PendingPayPalOrder> CreatePendingOrder(Guid accountId, decimal amount);

        Task CapturePendingOrder(string orderId, string payerId);
    }
}