﻿using System;
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
        CvarcClient client;

        public static void StartKrorServer(int port)
        {
            var process = new Process();
            process.StartInfo.FileName = "CVARC.exe";
            process.StartInfo.Arguments = "Debug " + port.ToString();
            process.Start();
            Thread.Sleep(100);
        }

        public TSensorData Configurate(int port, ConfigurationProposal configuration)
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect("127.0.0.1", port);
            client = new CvarcClient (tcpClient);
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

        public void Exit()
        {
            client.Close();
        }
    }
}
