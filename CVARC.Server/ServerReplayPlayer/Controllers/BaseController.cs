using System.Web.Mvc;
using ServerReplayPlayer.Logic;

namespace ServerReplayPlayer.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly Provider Provider = new Provider();

        protected new JsonResult Json(object obj)
        {
            var result = base.Json(obj);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }
    }
}
