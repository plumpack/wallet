using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlumPack.Web;

namespace PlumPack.Wallet.Web.Features.Account
{
    [Authorize]
    public class AccountController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}