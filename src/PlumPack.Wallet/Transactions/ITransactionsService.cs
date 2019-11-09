using System;
using System.Threading.Tasks;
using PlumPack.Wallet.Domain;

namespace PlumPack.Wallet.Transactions
{
    public interface ITransactionsService
    {
        Task<Transaction> AddTransaction(Guid accountId, decimal amount, string title, string metaData, Guid? payPalOrderId);
    }
}