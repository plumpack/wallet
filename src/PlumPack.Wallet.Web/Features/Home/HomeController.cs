using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlumPack.Wallet.Web.Features.Home
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TempData.Keep(); // Preserve success messages.
            return RedirectToAction("Index", "AddFunds");
        }
    }
}