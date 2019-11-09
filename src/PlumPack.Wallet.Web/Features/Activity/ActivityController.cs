using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlumPack.Web;

namespace PlumPack.Wallet.Web.Features.Activity
{
    [Authorize]
    public class ActivityController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}