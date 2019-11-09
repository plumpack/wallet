using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlumPack.Web;

namespace PlumPack.Wallet.Web.Features.Transfer
{
    [Authorize]
    public class TransferController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}