using System;
using System.Collections.Generic;

namespace CVARC.Basic.Controllers
{
    public abstract class Controller
    {
        public event Action<Command> ProcessCommand = command => { };
        protected void RaiseProcess(IEnumerable<Command> commands)
        {
            foreach(var command in commands)
                ProcessCommand(command);
        }
    }
}