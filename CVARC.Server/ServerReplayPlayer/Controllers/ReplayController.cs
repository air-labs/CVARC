using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Contexts;
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
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            //todo Завалидировать файл, что етьс run.bat и можем распаковать.
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
        public void SaveMatchResult()
        {
            var str = new StreamReader(Request.InputStream, Encoding.UTF8).ReadToEnd();
            var matchResult = JsonConvert.DeserializeObject<MatchResultServer>(str);
            _provider.SaveMatchResult(matchResult);
        }
    }
}
