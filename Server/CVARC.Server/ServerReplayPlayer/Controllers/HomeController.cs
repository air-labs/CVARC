using System.Web.Mvc;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string level)
        {
            var model = new CompetitionsViewModel
            {
                Command = Command,
                CompetitionsInfos = Provider.GetCompetitionsInfos(level),
                BackLevel = level
            };
            return View(model);
        }
    }
}
