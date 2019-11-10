using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using PlumPack.IdentityServer.Client;
using PlumPack.Wallet.Domain;

namespace PlumPack.Wallet.Accounts
{
    public static class AccountsServiceExtensions
    {
        public static Task<Account> GetAccount(this IAccountsService accountsService, ClaimsPrincipal user)
        {
            var userId = user.GetUserId();
            return accountsService.GetAccount(AccountIdentification.ByGlobalUserId(userId));
        }

        public static Task<Account> GetOrCreateAccount(this IAccountsService accountsService, ClaimsPrincipal user)
        {
            var userId = user.GetUserId();
            return accountsService.GetOrCreateAccount(AccountIdentification.ByGlobalUserId(userId));
        }
    }
}