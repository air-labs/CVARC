using System;
using ServerReplayPlayer.Logic;

namespace ServerReplayPlayer.Controllers
{
    public class LoginController : BaseController
    {
        public void Login(string login, string password)
        {
            if (!LoginProvider.TryLogin(Response, login, password))
            {
                Logger.InfoFormat("Try Login: {0} {1}", login, password);
                throw new Exception("Incorrect Login or Password");
            }
            Logger.InfoFormat("Success Login: {0}", login);
        }

        public void Logout()
        {
            LoginProvider.Logout(Response);
        }
    }
}
