using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class Controller<TCommand> : IController
    {
        public virtual void Initialize(IActor controllableActor)
        {
            
        }

        object IController.GetCommand()
        {
            return GetCommand();
        }

        public abstract TCommand GetCommand();

        public virtual void SendSensorData(object sensorData)
        {
        }
    }
}
