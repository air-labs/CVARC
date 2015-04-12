using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CommonTypes;
using Newtonsoft.Json;
using ServerReplayPlayer.Contracts;
using ServerReplayPlayer.Logic;
using ServerReplayPlayer.Logic.Providers;

namespace ServerReplayPlayer.Controllers
{
    public class ReplayController : BaseController
    {
        [HttpGet]
        public ActionResult Get(string level, Guid id, string redPoints, string bluePoints)
        {
            return View("PlayReplay", new ReplayViewModel(level, id, redPoints, bluePoints));
        }

        [HttpGet]
        public string GetReplay(string level, Guid id)
        {
            return Provider.GetReplay(level, id);
        }

        [HttpPost]
        public ActionResult UploadFile(string level, HttpPostedFileBase file, string backLevel)
        {
            if (Command == null)
                return View("AccessDenied");
            var client = ZipClientReader.Read(file);
            if (client == null)
                return View("FileFormatError");
            if (!DeadlineProvider.CanUploadClient(level))
                throw new Exception("Time for upload client is over!");
            Provider.AddPlayer(level, client, Command.CommandName);
            return RedirectToAction("Index", "Home", new { level = backLevel });
        }

        [HttpPost]
        public ActionResult GetPlayer(string level, Guid id)
        {
            return Json(Provider.GetPlayer(level, id));
        }

        [HttpPost]
        public JsonResult GetCompetitionsInfo(string level)
        {
            return Json(Provider.GetCompetitionsInfos(level).Single());
        }

        [HttpPost]
        public void SaveMatchResult(string level)
        {
            if (Command == null || !Command.IsAdmin)
                throw new Exception("Access Denied!");
            var str = new StreamReader(Request.InputStream, Encoding.UTF8).ReadToEnd();
            var matchResult = JsonConvert.DeserializeObject<MatchResult>(str);
            Provider.SaveMatchResult(level, matchResult);
        }
    }
}
