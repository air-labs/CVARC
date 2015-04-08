using System;
using System.Web;
using System.Web.Mvc;
using ServerReplayPlayer.Logic;

namespace ServerReplayPlayer.Controllers
{
    public class AdminController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (Command == null || !Command.IsAdmin)
                throw new Exception("Not Authorized");
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public void ChangeOpenLevel(string level)
        {
            Provider.ChangeOpenLevel(level);
        }

        [HttpPost]
        public void AddLogins(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength >= 0)
            {
                LoginProvider.AddLogins(file);
            }
        }

        [HttpPost]
        public void ChangeDeadlines(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength >= 0)
            {
                DeadlineProvider.ChangeDeadlines(file);
            }
        }
    }
}
