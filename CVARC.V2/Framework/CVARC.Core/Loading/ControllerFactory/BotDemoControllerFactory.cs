using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class BotDemoControllerFactory : ControllerFactory
    {

        override public IController Create(string controllerId, IActor actor)
        {
            return CreateBot(controllerId);
        }
    }
}
