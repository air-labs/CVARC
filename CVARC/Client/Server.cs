using System.Net.Sockets;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Basic.Core;
using CVARC.Basic.Core.Serialization;
using CVARC.Network;

namespace ClientBase
{
    public class Server<TSensorsData> where TSensorsData : ISensorsData
    {
        private readonly ClientSettings settings;
        private readonly ISerializer serializer = new JsonSerializer();
        private readonly NetworkStream stream;
        private int? robotId;

        public Server(ClientSettings settings)
        {
            this.settings = settings;
            var tcpClient = new TcpClient(settings.Ip, settings.Port);
            stream = tcpClient.GetStream();
        }

        public TSensorsData Run()
        {
            stream.Write(serializer.Serialize(GetHelloPackage()));
            stream.Flush();
            return serializer.Deserialize<TSensorsData>(stream.ReadBytes());
        }

        private HelloPackage GetHelloPackage()
        {
            return new HelloPackage
            {
                MapSeed = settings.MapNumber,
                Opponent = settings.BotName.ToString(),
                Side = settings.Side,
                LevelName = settings.LevelName
            };
        }

        public TSensorsData SendCommand(Command command = null)
        {
            if (command != null)
                SendCommandInternal(command);
            return serializer.Deserialize<TSensorsData>(stream.ReadBytes());
        }

        private void SendCommandInternal(Command command)
        {
//            if (robotId == null)
//                throw new Exception("Сервер не ининциализирован. Воспользуйтесь методом Run.");
//            command.RobotId = robotId.Value;
    //        var str = new string(Encoding.UTF8.GetChars(serializer.Serialize(command)));
            stream.Write(serializer.Serialize(command));
            stream.Flush();
        }
    }
}
