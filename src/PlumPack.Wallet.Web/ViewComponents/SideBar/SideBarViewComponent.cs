using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlumPack.IdentityServer.Client;
using PlumPack.Wallet.Accounts;
using PlumPack.Wallet.Web.ViewComponents.SideBar.Models;

namespace PlumPack.Wallet.Web.ViewComponents.SideBar
{
    public class SideBarViewComponent : ViewComponent
    {
        private readonly IAccountsService _accountsService;

        public SideBarViewComponent(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(string selectedTab)
        {
            var account = await _accountsService.GetAccount(AccountIdentification.ByGlobalUserId(((ClaimsPrincipal)User).GetUserId()));
            
            return View(new SideBarViewModel
            {
                SelectedTab = selectedTab,
                Amount = account?.CurrentBalance ?? 0
            });
        }
    }
}