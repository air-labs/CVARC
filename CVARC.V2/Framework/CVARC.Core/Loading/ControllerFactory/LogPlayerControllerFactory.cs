using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class LogPlayerControllerFactory : IControllerFactory
    {
        Log log;

        public LogPlayerControllerFactory(Log log)
        {
            this.log = log;
        }

        public IController Create(ControllerRequest request)
        {
            if (!log.Commands.ContainsKey(request.ControllerId))
                throw new Exception("The log does not contain records for '"+request.ControllerId+"'");
            return new LogPlayController(log.Commands[request.ControllerId]);
        }
    }
}
