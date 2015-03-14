using System.Web.Mvc;

namespace ServerReplayPlayer.Controllers
{
    public abstract class BaseController : Controller
    {
        protected new JsonResult Json(object obj)
        {
            var result = base.Json(obj);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }
    }
}
