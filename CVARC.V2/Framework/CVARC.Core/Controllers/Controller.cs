using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class Controller<TCommand> : IController
        where TCommand : ICommand
    {
        public virtual void Initialize(IActor controllableActor)
        {
            
        }

        ICommand IController.GetCommand(Type CommandType)
        {
            return GetCommand();
        }

        public abstract TCommand GetCommand();

        public virtual void SendSensorData(object sensorData)
        {
        }
    }
}
