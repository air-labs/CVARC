using System.Web.Mvc;

namespace ServerReplayPlayer.Controllers
{
    public class StatisticsController : BaseController
    {
        public ActionResult Index()
        {
            var info = Provider.GetCompetitionsInfos(null);
            return View();
        }

    }
}
