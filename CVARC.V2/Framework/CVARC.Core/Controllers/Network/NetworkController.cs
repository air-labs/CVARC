using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class NetworkController : IController
    {
        GroboTcpClient client;

        public NetworkController(GroboTcpClient client)
        {
            this.client = client;
        }

        public void Initialize(IActor controllableActor)
        {
           
        }

        public ICommand GetCommand()
        {
            return null;
        }

        public void SendSensorData(object sensorData)
        {
            
        }
    }
}
