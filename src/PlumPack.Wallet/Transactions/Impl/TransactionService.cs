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
                // Let's update the total on the account object.
                var runningTotal = await con.Connection.SingleAsync<decimal>(
                    con.Connection.From<Transaction>()
                        .Select(x => new {
                            Sum = Sql.Sum(x.Amount)
                        }));
                
                runningTotal += amount;
                
                var transaction = new Transaction
                {
                    AccountId = accountId,
                    Date = DateTimeOffset.UtcNow,
                    Amount = amount,
                    CurrentBalance = runningTotal,
                    Title = title,
                    MetaData = metaData,
                    PayPalOrderId = payPalOrderId
                };

                await con.Connection.SaveAsync(transaction);

                await con.Connection.UpdateOnlyAsync(() => new Account{CurrentBalance = runningTotal}, x => x.Id == accountId);
                
                trans.Commit();
                
                return transaction;
            }
        }

        public async Task<SeekedList<Transaction>> GetTransactions(Guid? accountId = null, int? skip = null, int? take = null)
        {
            using (var con = new ConScope(_dataService))
            {
                var query = con.Connection.From<Transaction>();

                if (accountId.HasValue)
                {
                    query.Where(x => x.AccountId == accountId);
                }
                
                long? count = null;
                if (!skip.HasValue && !take.HasValue)
                {
                    // We are returning all the items, not need to do additional DB query.
                }
                else
                {
                    count = await con.Connection.CountAsync(query);
                }

                query.OrderByDescending(x => x.Date);
                
                if (skip.HasValue)
                {
                    query.Skip(skip.Value);
                }

                if (take.HasValue)
                {
                    query.Take(take.Value);
                }

                var results = await con.Connection.SelectAsync(query);

                if (!count.HasValue)
                {
                    count = results.Count;
                }
                
                return new SeekedList<Transaction>(results, skip, take, count.Value);
            }
        }
    }
}