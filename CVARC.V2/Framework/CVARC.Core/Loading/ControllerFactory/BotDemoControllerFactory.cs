using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class BotDemoControllerFactory : IControllerFactory
    {

        public IController Create(ControllerRequest request)
        {
            return request.CreateBot();
        }
    }
}
