using System;

namespace ServerReplayPlayer.Controllers
{
    public class LoginController : BaseController
    {
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
