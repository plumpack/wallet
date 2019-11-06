using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PlumPack.Infrastructure;
using PlumPack.Wallet.Domain;
using ServiceStack.OrmLite;
using SharpDataAccess.Data;

namespace PlumPack.Wallet.Accounts.Impl
{
    [Service(typeof(IAccountsService))]
    public class AccountsService : IAccountsService
    {
        private readonly IDataService _dataService;
        private readonly ILogger<AccountsService> _logger;

        public AccountsService(IDataService dataService,
            ILogger<AccountsService> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        public async Task<Account> GetAccount(AccountIdentification id)
        {
            using (var con = new ConScope(_dataService))
            {
                if (id.Id.HasValue)
                {
                    return await con.Connection.SingleAsync<Account>(id.Id);
                }

                if (id.GlobalUserId.HasValue)
                {
                    return await con.Connection.SingleAsync<Account>(x => x.GlobalUserId == id.GlobalUserId);
                }
                
                throw new Exception("Invalid id");
            }
        }

        public async Task<Account> GetOrCreateAccount(AccountIdentification id)
        {
            using (var con = new ConScope(_dataService))
            using (var trans = await con.BeginTransaction())
            {
                var account = await GetAccount(id);
                
                if (account == null)
                {
                    if (!id.GlobalUserId.HasValue)
                    {
                        throw new Exception("Can only auto create account when using global user id.");
                    }

                    account = new Account
                    {
                        GlobalUserId = id.GlobalUserId.Value
                    };

                    await con.Connection.SaveAsync(account);
                    
                    trans.Commit();
                    
                    _logger.LogInformation("Created account {AccountId} for global user {GlobalUserId", account.Id, id.GlobalUserId);
                }

                return account;
            }
        }
    }
}