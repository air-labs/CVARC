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
                BotName = Bots.Azura,
                Side = Side.Left,
                LevelName = LevelName.Level2,
                MapNumber = 5
            };

        private static void Main(string[] args)
        {
            var server = new CvarcClient(args, Settings).GetServer<PositionSensorsData>();
            var sensorData = server.Run();
            var map = sensorData.BuildMap();
            var robotLocator = new RobotLocator(map);
            var path = PathSearcher.FindPath(map, new Point(1, 1), new Point(2, 1));

            foreach (var direction in path)
            {
                foreach (var command in robotLocator.GetCommandsByDirection(direction))
                {
                    sensorData = server.SendCommand(command);
                    robotLocator.Update(sensorData);
                }
            }
            server.SendCommand(new Command { Time = 10000 });
        }
    }
}
