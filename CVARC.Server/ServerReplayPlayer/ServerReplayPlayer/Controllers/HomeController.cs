using System.Web.Mvc;

namespace ServerReplayPlayer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string GetReplay()
        {
            string path = ControllerContext.HttpContext.Server.MapPath("~\\Replays\\log.replay.json");
            var replay = System.IO.File.ReadAllText(path);
            return replay;
        }
    }
}
