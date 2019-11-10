using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlumPack.Wallet.Accounts;
using PlumPack.Wallet.Domain;
using PlumPack.Wallet.Transactions;
using PlumPack.Wallet.Web.Features.Activity.Models;
using PlumPack.Web;

namespace PlumPack.Wallet.Web.Features.Activity
{
    [Authorize]
    public class ActivityController : BaseController
    {
        private readonly ITransactionsService _transactionsService;
        private readonly IAccountsService _accountsService;

        public ActivityController(ITransactionsService transactionsService,
            IAccountsService accountsService)
        {
            _transactionsService = transactionsService;
            _accountsService = accountsService;
        }
        
        public async Task<ActionResult> Index()
        {
            var vm = new ActivityViewModel();
            var account = await _accountsService.GetAccount(User);

            if (account == null)
            {
                vm.Transactions = new List<Transaction>();
            }
            else
            {
                vm.Transactions = await _transactionsService.GetTransactions(account.Id);
            }

            return View(vm);
        }
    }
}