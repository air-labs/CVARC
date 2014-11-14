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
        CvarcTcpClient client;

        public TSensorData Configurate(bool runServer, int port, ConfigurationProposal configuration)
        {
            if (runServer)
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = "CVARC.exe",
                        Arguments = "Debug " + port.ToString()
                    }
                };
                process.Start();
                Thread.Sleep(500);
            }
            var tcpClient = new TcpClient();
            tcpClient.Connect("127.0.0.1", port);
            client = new CvarcTcpClient(tcpClient);
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
