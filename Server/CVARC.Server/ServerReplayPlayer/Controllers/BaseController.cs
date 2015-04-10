using System.Web.Mvc;
using ServerReplayPlayer.Contracts;
using ServerReplayPlayer.Logic;
using ServerReplayPlayer.Logic.Providers;

namespace ServerReplayPlayer.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly Provider Provider = new Provider();
        protected readonly LoginProvider LoginProvider = new LoginProvider();
        protected CommandEntity Command { get; private set; }

        protected new JsonResult Json(object obj)
        {
            var result = base.Json(obj);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            Command = LoginProvider.Auth(Request);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            Logger.Error(filterContext.Exception);
        }
    }
}
