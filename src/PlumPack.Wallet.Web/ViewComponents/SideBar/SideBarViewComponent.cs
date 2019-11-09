using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlumPack.Wallet.Web.ViewComponents.SideBar.Models;

namespace PlumPack.Wallet.Web.ViewComponents.SideBar
{
    public class SideBarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string selectedTab)
        {
            return View(new SideBarViewModel
            {
                SelectedTab = selectedTab,
                Amount = 23.00m
            });
        }
    }
}