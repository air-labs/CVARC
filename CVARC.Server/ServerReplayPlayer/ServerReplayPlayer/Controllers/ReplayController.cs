using System.Web;
using System.Web.Mvc;
using ServerReplayPlayer.Contracts;
using ServerReplayPlayer.Logic;

namespace ServerReplayPlayer.Controllers
{
    public class ReplayController : BaseController
    {
        private readonly Provider _provider = new Provider();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                Provider.AddPlayer(file);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetPlayer(string name)
        {
            return Json(_provider.GetPlayer(name));
        }

        [HttpPost]
        public JsonResult GetCompetitionsInfo()
        {
            return Json(_provider.GetCompetitionsInfo());
        }

        [HttpPost]
        public void SaveMatchResult(MatchResultServer matchResult)
        {
            _provider.SaveMatchResult(matchResult);
        }
    }
}
