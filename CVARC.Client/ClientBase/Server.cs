using System.Net.Sockets;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Basic.Core.Participants;
using CVARC.Basic.Core.Serialization;
using CVARC.Network;

namespace ClientBase
{
    public class Server<TSensorsData> where TSensorsData : ISensorsData
    {
        private readonly ClientSettings settings;
        private readonly ISerializer serializer = new JsonSerializer();
        private readonly GroboTcpClient client;
        private int? robotId;

        public Server(ClientSettings settings)
        {
            this.settings = settings;
            var tcpClient = new TcpClient(settings.Ip, settings.Port);
            client = new GroboTcpClient(tcpClient);
        }

        public TSensorsData Run()
        {
            client.Send(serializer.Serialize(GetHelloPackage()));
            return serializer.Deserialize<TSensorsData>(client.ReadToEnd());
        }

        private HelloPackage GetHelloPackage()
        {
            return new HelloPackage
            {
                MapSeed = settings.MapNumber,
                Opponent = settings.BotName.ToString(),
                Side = settings.Side,
                LevelName = settings.LevelName.ToString()
            };
        }

        public TSensorsData SendCommand(Command command = null)
        {
            if (command != null)
                SendCommandInternal(command);
            return serializer.Deserialize<TSensorsData>(client.ReadToEnd());
        }

        private void SendCommandInternal(Command command)
        {
//            if (robotId == null)
//                throw new Exception("Сервер не ининциализирован. Воспользуйтесь методом Run.");
//            command.RobotId = robotId.Value;
    //        var str = new string(Encoding.UTF8.GetChars(serializer.Serialize(command)));
            client.Send(serializer.Serialize(command));
        }
    }
}
