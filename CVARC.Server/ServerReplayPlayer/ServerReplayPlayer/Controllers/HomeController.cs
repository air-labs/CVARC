using System.IO;
using System.Linq;
using System.Web.Mvc;
using ServerReplayPlayer.Models;

namespace ServerReplayPlayer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(ReadReplays());
        }

        [HttpGet]
        public string GetReplay(string name)
        {
            string path = ControllerContext.HttpContext.Server.MapPath("~\\Replays\\");
            var replay = System.IO.File.ReadAllLines(path + name).Last();
            return replay;
        }

        private Log[] ReadReplays()
        {
            string path = ControllerContext.HttpContext.Server.MapPath("~\\Replays");
            return Directory.GetFiles(path).Select(x =>
                {
                    var side = x.Contains("Blue") ? 1 : 0;
                    using (var file = new StreamReader(x))
                        return new Log
                            {
                                Language = file.ReadLine(),
                                Points =  file.ReadLine().Split(':')[side],
                                Name = Path.GetFileName(x),
                                Time = file.ReadLine() + " sec"
                            };
                }).OrderBy(x => x.Time).ToArray();
        }
    }
}
