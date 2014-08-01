using CVARC.Basic.Controllers;
using CVARC.Network;
using ClientBase;
using RepairTheStarship.Sensors;
using MapHelper;

namespace Level2Client
{
    internal class Program
    {
        private static readonly ClientSettings Settings = new ClientSettings
            {
                BotName = Bots.MolagBal,
                Side = Side.Left,
                LevelName = "Level1"
            };

        private static void Main(string[] args)
        {
            var server = new CvarcClient(args, Settings).GetServer<SensorsData>();
            var sensorData = server.Run();
            var map = sensorData.BuildStaticMap();
            var robotLocator = new RobotLocator(map);
            var path = PathSearcher.FindPath(map, new Point(1, 1), new Point(2, 1));

//            foreach (var direction in path)
//            {
//                foreach (var command in robotLocator.GetCommandsByDirection(direction))
//                {
//                    sensorData = server.SendCommand(command);
//                    robotLocator.Update(sensorData);
//                }
//            }
            server.SendCommand(new Command { Time = 10000 });
        }
    }
}
