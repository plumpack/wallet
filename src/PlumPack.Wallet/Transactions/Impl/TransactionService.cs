using System;
using System.Threading.Tasks;
using PlumPack.Infrastructure;
using PlumPack.Wallet.Domain;
using ServiceStack.OrmLite;
using SharpDataAccess.Data;

namespace PlumPack.Wallet.Transactions.Impl
{
    [Service(typeof(ITransactionsService))]
    public class TransactionService : ITransactionsService
    {
        private readonly IDataService _dataService;

        public TransactionService(IDataService dataService)
        {
            _dataService = dataService;
        }
        
        public async Task<Transaction> AddTransaction(Guid accountId, decimal amount, string title, string metaData, Guid? payPalOrderId)
        {
            using (var con = new ConScope(_dataService))
            {
                var transaction = new Transaction
                {
                    AccountId = accountId,
                    Amount = amount,
                    Title = title,
                    MetaData = metaData,
                    PayPalOrderId = payPalOrderId
                };

                await con.Connection.SaveAsync(transaction);

                return transaction;
            }
        }
    }
}