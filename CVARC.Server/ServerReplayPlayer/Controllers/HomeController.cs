using System.Web.Mvc;

namespace ServerReplayPlayer.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string level)
        {
            return View(Provider.GetCompetitionsInfos(level));
        }

        public ActionResult Test()
        {
            return View("Index", Provider.GetTestCompetitionsInfos());
        }
    }
}
