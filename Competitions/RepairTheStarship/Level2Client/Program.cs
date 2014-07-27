using System.Collections.Generic;
using CVARC.Basic.Controllers;
using CVARC.Network;
using ClientBase;
using RepairTheStarship.Sensors;
using MapHelper;

namespace Level2Client
{
    internal class Program
    {
        private static Direction currentDirection = Direction.Right;
        private static double realRobotAngle = currentDirection.ToAngle();
        private static double expectedRobotAngle = realRobotAngle;

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
            var path = PathSearcher.FindPath(map, new Point(1, 1), new Point(2, 1));

            foreach (var command in GetNextCommand(path))
            {
                sensorData = server.SendCommand(command);
                map.Update(sensorData);
                realRobotAngle = map.CurrentRobotAngle;
            }
            server.SendCommand(new Command { Time = 10000 });
        }

        private static IEnumerable<Command> GetNextCommand(Direction[] path)
        {
            foreach (var nextDirection in path)
            {
                currentDirection = nextDirection;
                yield return CorrectRobotPosition();
                yield return Command.Rot(nextDirection.ToAngle() - expectedRobotAngle);
                expectedRobotAngle = currentDirection.ToAngle();
                yield return CorrectRobotPosition();
                yield return Command.Mov(50);
            }
        }

        private static Command CorrectRobotPosition()
        {
            var angleError = expectedRobotAngle - realRobotAngle;
            return Command.Rot(angleError); 
        }
    }
}
