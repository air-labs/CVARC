using System.Net.Sockets;
using System.Text;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Basic.Core;
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

        public Server(ClientSettings settings)
        {
            this.settings = settings;
            var tcpClient = new TcpClient(settings.Ip, settings.Port);
            client = new GroboTcpClient(tcpClient);
        }

        public HelloPackageAns Run()
        {
            client.Send(serializer.Serialize(GetHelloPackage()));
            return new HelloPackageAns
                {
                    RealSide = Encoding.UTF8.GetString(client.ReadToEnd()).ParseEnum<Side>(),
                    SensorsData = serializer.Deserialize<TSensorsData>(client.ReadToEnd())
                };
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

        public TSensorsData SendCommand(Command command)
        {
            client.Send(serializer.Serialize(command));
            return serializer.Deserialize<TSensorsData>(client.ReadToEnd());
        }

        public class HelloPackageAns
        {
            public TSensorsData SensorsData { get; set; }
            public Side RealSide { get; set; }
        }
    }
}
