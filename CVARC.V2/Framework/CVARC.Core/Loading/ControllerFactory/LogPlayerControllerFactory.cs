using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class LogPlayerControllerFactory : ControllerFactory
    {
        Log log;

        public LogPlayerControllerFactory(Log log)
        {
            this.log = log;
        }

        override public IController Create(string controllerId, IActor actor)
        {
            if (!log.Commands.ContainsKey(controllerId))
                throw new Exception("The log does not contain records for '" + controllerId + "'");
            return new LogPlayController(log.Commands[controllerId]);
        }
    }
}
