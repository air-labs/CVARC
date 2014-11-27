using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CVARC.V2
{
    public class CvarcClient<TSensorData, TCommand> 
        where TSensorData : class
    {
        CvarcClientSideTcpClient client;

        public TSensorData Configurate(int port, ConfigurationProposal configuration)
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect("127.0.0.1", port);
            client = new CvarcClientSideTcpClient(tcpClient);
            client.Write(configuration);
            return client.Read<TSensorData>();
        }

        

        public TSensorData Act(TCommand command)
        {
                client.Write(command);
                var sensorData = client.Read<TSensorData>();
                if (sensorData == null)
                    Environment.Exit(0);
                return sensorData;
        }
    }
}
