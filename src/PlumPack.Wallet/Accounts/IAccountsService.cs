using System;
using System.Threading.Tasks;
using PlumPack.Wallet.Domain;

namespace PlumPack.Wallet.Accounts
{
    public interface IAccountsService
    {
        Task<Account> GetAccount(AccountIdentification id);
        
        Task<Account> GetOrCreateAccount(AccountIdentification id);
    }
}