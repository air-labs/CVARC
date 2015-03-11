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
        private const string Level = "1";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (FileValidator.IsValid(file))
            {
                Provider.AddPlayer(Level, file);
                return RedirectToAction("Index");
            }
            return View("FileFormatError");
        }

        [HttpPost]
        public ActionResult GetPlayer(Guid id)
        {
            return Json(_provider.GetPlayer(Level, id));
        }

        [HttpPost]
        public JsonResult GetCompetitionsInfo()
        {
            return Json(_provider.GetCompetitionsInfo(Level));
        }

        [HttpPost]
        public void SaveMatchResult()
        {
            var str = new StreamReader(Request.InputStream, Encoding.UTF8).ReadToEnd();
            var matchResult = JsonConvert.DeserializeObject<MatchResultContract>(str);
            _provider.SaveMatchResult(Level, matchResult);
        }
    }
}
