using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class NetworkController : IController
    {
        GroboTcpClient client;
        private static readonly ISerializer Serializer = new JsonSerializer();
    
        public NetworkController(GroboTcpClient client)
        {
            this.client = client;
        }

        public void Initialize(IActor controllableActor)
        {
           
        }

        object sensorData;
        public ICommand GetCommand(Type CommandType)
        {
            client.Send(Serializer.Serialize(sensorData));
            var command = (ICommand)Serializer.Deserialize(CommandType, client.ReadToEnd());
            return command;
        }

        public void SendSensorData(object sensorData)
        {
            this.sensorData = sensorData;   
        }
    }
}
