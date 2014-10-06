using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class KeyboardController<TCommand> : IController<TCommand>
        where TCommand : ICommand
    {
        public KeyboardController(KeyboardControllerPool<TCommand> pool, string controllerId)
        {
            this.pool = pool;
            this.controllerId = controllerId;
        }

        KeyboardControllerPool<TCommand> pool;
        string controllerId; 

        public TCommand GetCommand()
        {
            return pool.GetCommand(controllerId);
        }

        public void Initialize(IControllable controllableActor)
        {
            
        }

    }
}
