using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class BotDemoEnvironment : StandardEnvironment
    {

        public override void PrepareControllers(string[] allControllersId)
        {
            
        }

        public override IController GetController(string controllerId)
        {
            if (!commandLineArgs.ContainsKey(controllerId))
                throw new Exception(string.Format("The bot for controller '{0}' was not specified", controllerId));
            var botName = commandLineArgs[controllerId];
            if (!competitions.Logic.Bots.ContainsKey(botName))
                throw new Exception(string.Format("The bot '{0}' specified for controller '{1}' is not defined", botName, controllerId));
            var bot = competitions.Logic.Bots[botName]();
            return bot;
        }
    }
}
