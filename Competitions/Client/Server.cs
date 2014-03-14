using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using CVARC.Basic;
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
        private NetworkStream stream;
        private int? robotId;

        public Server(string ip, int port)
        {
            var tcpClient = new TcpClient(ip, port);
            stream = tcpClient.GetStream();
            streamReader = new StreamReader(stream);
            streamWriter = new StreamWriter(stream);
        }

        public SensorsData Run(HelloPackage helloPackage)
        {
            streamWriter.BaseStream.Write(serializer.Serialize(helloPackage));
            streamWriter.Flush();
            return serializer.Deserialize<SensorsData>(stream.ReadBytes());
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
                throw new Exception("Сервер не ининциализирован. Воспользуйтесь методом Run.");
            command.RobotId = robotId.Value;
            streamWriter.Write(command.Serialize());
            streamWriter.Flush();
        }
    }
}
