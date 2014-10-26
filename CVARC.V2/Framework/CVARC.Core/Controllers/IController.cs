using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface IController
    {
        void Initialize(IActor controllableActor);
        object GetCommand();
        void SendSensorData(object sensorData);
    }
}
