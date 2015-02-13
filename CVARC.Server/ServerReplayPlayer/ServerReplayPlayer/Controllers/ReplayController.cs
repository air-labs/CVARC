using System.Web;
using System.Web.Mvc;
using ServerReplayPlayer.Contracts;
using ServerReplayPlayer.Logic;

namespace ServerReplayPlayer.Controllers
{
    public class ReplayController : Controller
    {
        private readonly Storage storage = new Storage();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                Storage.AddPlayer(file);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetPlayer(string name)
        {
            return Json(storage.GetPlayer(name));
        }

        [HttpPost]
        public JsonResult GetCompetitionsInfo()
        {
            return Json(storage.GetCompetitionsInfo());
        }

        [HttpPost]
        public void SaveMatchResult(MatchResultServer matchResult)
        {
            storage.SaveMatchResult(matchResult);
        }
    }
}
