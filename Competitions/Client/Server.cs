using System;
using System.IO;
using System.Net.Sockets;
using CVARC.Basic.Controllers;
using CVARC.Basic.Core;
using CVARC.Basic.Core.Serialization;
using CVARC.Network;

namespace Client
{
    class Server
    {
        private readonly ISerializer serializer = new JsonSerializer();
        private readonly StreamReader streamReader;
        private readonly StreamWriter streamWriter;
        private int? robotId;

        public Server(string ip, int port)
        {
            var tcpClient = new TcpClient(ip, port);
            streamReader = new StreamReader(tcpClient.GetStream());
            streamWriter = new StreamWriter(tcpClient.GetStream());
        }

        public void Run(HelloPackage helloPackage)
        {
            streamWriter.BaseStream.Write(serializer.Serialize(helloPackage));
            streamWriter.Flush();
            Console.WriteLine(streamReader.ReadLine());
        }

        public string GetSensorData(Command command = null)
        {
            if (command != null)
                SendCommand(command);
            return streamReader.ReadLine();
        }

        private void SendCommand(Command command)
        {
            if (robotId == null)
                throw new Exception("Севрер не ининциализирован. Воспользуйтесь методом Run.");
            command.RobotId = robotId.Value;
            streamWriter.Write(command.Serialize());
            streamWriter.Flush();
        }
    }
}
