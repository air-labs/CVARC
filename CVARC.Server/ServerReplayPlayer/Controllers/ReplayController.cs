using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ServerReplayPlayer.Contracts;
using ServerReplayPlayer.Logic;

namespace ServerReplayPlayer.Controllers
{
    public class ReplayController : BaseController
    {
        private readonly Provider provider = new Provider();

        [HttpGet]
        public ActionResult Get(string level, Guid id)
        {
            return View("PlayReplay", new ReplayViewModel(level, id));
        }

        [HttpGet]
        public string GetReplay(string level, Guid id)
        {
            return provider.GetReplay(level, id);
        }

        [HttpGet]
        public ActionResult UploadFailed()
        {
            return View("FileFormatError");
        }

        [HttpPost]
        public void UploadFile(string level, HttpPostedFileBase file)
        {
            if (!FileValidator.IsValid(file))
            {
                provider.SaveInvalidClient(file);
                throw new Exception("File is invalid!");
            }
            provider.AddPlayer(level, file);
        }

        [HttpPost]
        public ActionResult GetPlayer(string level, Guid id)
        {
            return Json(provider.GetPlayer(level, id));
        }

        [HttpPost]
        public JsonResult GetCompetitionsInfo(string level)
        {
            return Json(provider.GetCompetitionsInfo(level));
        }

        [HttpPost]
        public void SaveMatchResult(string level)
        {
            var str = new StreamReader(Request.InputStream, Encoding.UTF8).ReadToEnd();
            var matchResult = JsonConvert.DeserializeObject<MatchResult>(str);
            provider.SaveMatchResult(level, matchResult);
        }
    }
}
