using System.Web.Mvc;
using ServerReplayPlayer.Logic;

namespace ServerReplayPlayer.Controllers
{
    public class HomeController : BaseController
    {
        private readonly Provider provider = new Provider();

        public ActionResult Index()
        {
            return View(provider.GetCompetitionsInfos());
        }

        public ActionResult Test()
        {
            return View("Index", provider.GetTestCompetitionsInfos());
        }
    }
}
