using System.Web.Mvc;

namespace ServerReplayPlayer.Controllers
{
    public class AdminController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public void ChangeOpenLevel(string level)
        {
            Provider.ChangeOpenLevel(level);
        }
    }
}
