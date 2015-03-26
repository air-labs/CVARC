using System;
using System.Web.Mvc;

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
    }
}
