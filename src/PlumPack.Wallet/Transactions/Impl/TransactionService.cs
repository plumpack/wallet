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
            using (var trans = await con.BeginTransaction())
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

                // Let's update the total on the account object.
                var runningTotal = await con.Connection.SingleAsync<decimal>(
                    con.Connection.From<Transaction>()
                        .Select(x => new {
                            Sum = Sql.Sum(x.Amount)
                        }));
                
                await con.Connection.UpdateOnlyAsync(() => new Account{CurrentBalance = runningTotal}, x => x.Id == accountId);
                
                trans.Commit();
                
                return transaction;
            }
        }
    }
}