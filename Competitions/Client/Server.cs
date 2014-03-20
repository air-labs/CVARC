using System;
using System.Net.Sockets;
using CVARC.Basic.Controllers;
using CVARC.Basic.Core;
using CVARC.Basic.Core.Serialization;
using CVARC.Network;

namespace Client
{
    class Server<TSensorsData>
    {
        private readonly ISerializer serializer = new JsonSerializer();
        private readonly NetworkStream stream;
        private int? robotId;

        public Server(string ip, int port)
        {
            var tcpClient = new TcpClient(ip, port);
            stream = tcpClient.GetStream();
        }

        public TSensorsData Run(HelloPackage helloPackage)
        {
            stream.Write(serializer.Serialize(helloPackage));
            stream.Flush();
            return serializer.Deserialize<TSensorsData>(stream.ReadBytes());
        }

        public TSensorsData GetSensorData(Command command = null)
        {
            if (command != null)
                SendCommand(command);
            return serializer.Deserialize<TSensorsData>(stream.ReadBytes());
        }

        private void SendCommand(Command command)
        {
            if (robotId == null)
                throw new Exception("Сервер не ининциализирован. Воспользуйтесь методом Run.");
            command.RobotId = robotId.Value;
            stream.Write(serializer.Serialize(command));
            stream.Flush();
        }
    }
}
