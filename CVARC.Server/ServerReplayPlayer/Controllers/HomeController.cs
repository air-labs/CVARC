using System;
using System.Web.Mvc;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string level)
        {
            var model = new CompetitionsViewModel
            {
                Command = Command,
                CompetitionsInfos = Provider.GetCompetitionsInfos(level)
            };
            return View(model);
        }

        public ActionResult Test()
        {
            return View("Index", new CompetitionsViewModel {CompetitionsInfos = Provider.GetTestCompetitionsInfos()});
        }

        public void Login(string login, string password)
        {
            if (!LoginProvider.TryLogin(Response, login, password))
                throw new Exception("Incorrect Login or Password");
        }

        public void Logout()
        {
            LoginProvider.Logout(Response);
        }
    }
}
