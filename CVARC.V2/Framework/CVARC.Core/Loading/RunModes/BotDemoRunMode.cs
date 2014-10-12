using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class BotDemoRunMode : StandardRunMode
    {

        public override IController GetController(string controllerId)
        {
            var record = this.GetControllerConfigFor(controllerId);
            if (record.Type != ControllerType.Bot)
                throw new Exception("Only bots should be specified for this mode, e.g. -Controller.Left Bot.Azura");
            return this.GetBotFor(record);
       
        }
    }
}
