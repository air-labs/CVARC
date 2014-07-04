using System;
using System.Linq;
using AIRLab.Mathematics;
using CVARC.Basic.Controllers;
using CVARC.Basic.Sensors;
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

        private static int currentCommand = 0;
        private static Direction currentDirection = Direction.Right;

        private static void Main(string[] args)
        {
            var server = new CvarcClient(args, Settings).GetServer<SensorsData>();
            var sensorData = server.Run();
            var robotId = sensorData.RobotIdSensor.Id;
            var staticMap = new MapBuilder().BuildStaticMap(sensorData.MapSensor.MapItems);
            var path = PathSearcher.FindPath(staticMap, new Point(1, 1), new Point(4, 4));

            while (true)
            {
                sensorData = server.GetSensorData(GetCommandByDirection(path));
                staticMap.Update(sensorData.MapSensor.MapItems);
            }
        }

        private static Command GetCommandByDirection(Direction[] path)
        {
            var distance = 50;
            if (path.Length == currentCommand)
                return new Command {Time = 1};
            if (path[currentCommand] != currentDirection)
            {
                var command =  ChangeDirection(path[currentCommand]);
                currentDirection = path[currentCommand];
                return command;
            }
            currentCommand++;
            return new Command(){Move = distance, Time = 1};
        }

        private static Command ChangeDirection(Direction needDirection)
        {
            int angle;
            if ((needDirection == Direction.Left && currentDirection == Direction.Right) ||
                (needDirection == Direction.Right && currentDirection == Direction.Left) ||
                (needDirection == Direction.Up && currentDirection == Direction.Down) ||
                (needDirection == Direction.Down && currentDirection == Direction.Up))
                angle = 180;
            else if ((needDirection == Direction.Left && currentDirection == Direction.Down) ||
                     (needDirection == Direction.Down && currentDirection == Direction.Right) ||
                     (needDirection == Direction.Right && currentDirection == Direction.Up) ||
                     (needDirection == Direction.Up && currentDirection == Direction.Left))
                angle = -90;
            else
                angle = 90;
            return new Command() {Angle = Angle.FromGrad(angle), Time = 1};
        }
    }
}
