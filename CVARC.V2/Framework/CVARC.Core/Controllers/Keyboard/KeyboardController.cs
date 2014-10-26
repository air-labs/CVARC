using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class KeyboardController<TCommand> : Controller<TCommand>
    {
        public KeyboardController(KeyboardControllerPool<TCommand> pool, string controllerId)
        {
            this.pool = pool;
            this.controllerId = controllerId;
        }

        KeyboardControllerPool<TCommand> pool;
        string controllerId; 

        override public TCommand GetCommand()
        {
            return pool.GetCommand(controllerId);
        }

    }
}
