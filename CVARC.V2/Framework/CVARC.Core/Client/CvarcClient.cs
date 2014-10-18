using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CVARC.V2
{
    public class CvarcClient<TSensorData, TCommand> 
        where TSensorData : class
    {
        CvarcTcpClient client;

        public CvarcClient()
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect("127.0.0.1", 14000);
            client = new CvarcTcpClient(tcpClient);
        }

        public TSensorData Configurate(ConfigurationProposal configuration)
        {
            client.SerializeAndSend(configuration);
            return client.ReadObject<TSensorData>();
        }

        public TSensorData Act(TCommand command)
        {
                client.SerializeAndSend(command);
                var sensorData = client.ReadObject<TSensorData>();
                if (sensorData == null)
                    Environment.Exit(0);
                return sensorData;
        }
    }
}
