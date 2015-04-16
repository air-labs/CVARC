using System.Web.Mvc;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Controllers
{
    public class StatisticsController : BaseController
    {
        public ActionResult Index()
        {
            string[] levels;
            var result = Provider.GetResult(LoginProvider.GetTeams(), out levels);
            return View(new StatisticsViewModel
                {
                    TeamResults = result,
                    Levels = levels
                });
        }
    }
}
