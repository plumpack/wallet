using System.Threading.Tasks;
using PayPalCheckoutSdk.Orders;

namespace PlumPack.Wallet.PayPal
{
    public interface IPayPalService
    {
        string ClientId { get; }

        Task<Order> CreateTransaction();
    }
}