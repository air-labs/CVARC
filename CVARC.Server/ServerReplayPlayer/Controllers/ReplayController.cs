using System;
using System.IO;
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
        private readonly Provider _provider = new Provider();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(string level, HttpPostedFileBase file)
        {
            if (FileValidator.IsValid(file))
            {
                Provider.AddPlayer(level, file);
                return RedirectToAction("Index");
            }
            return View("FileFormatError");
        }

        [HttpPost]
        public ActionResult GetPlayer(string level, Guid id)
        {
            return Json(_provider.GetPlayer(level, id));
        }

        [HttpPost]
        public JsonResult GetCompetitionsInfo(string level)
        {
            return Json(_provider.GetCompetitionsInfo(level));
        }

        [HttpPost]
        public void SaveMatchResult(string level)
        {
            var str = new StreamReader(Request.InputStream, Encoding.UTF8).ReadToEnd();
            var matchResult = JsonConvert.DeserializeObject<MatchResult>(str);
            _provider.SaveMatchResult(level, matchResult);
        }
    }
}
