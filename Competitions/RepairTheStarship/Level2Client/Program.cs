using System;
using System.Collections.Generic;
using AIRLab.Mathematics;
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
        private static double currentRobotAngle = DirectionToAngle(currentDirection);
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
            var map = new MapBuilder().BuildStaticMap(sensorData);
            var path = PathSearcher.FindPath(map, new Point(1, 1), new Point(4, 4));

            foreach (var command in GetNextCommand(path))
            {
                sensorData = server.GetSensorData(command);
                map.Update(sensorData);
                currentRobotAngle = map.CurrentRobotAngle;
            }
            server.GetSensorData(new Command { Time = 10000 });
        }

        private static IEnumerable<Command> GetNextCommand(Direction[] path)
        {
            foreach (var nextDirection in path)
            {
                var currentDirectionAngle = DirectionToAngle(currentDirection);
                var angle = currentDirectionAngle - DirectionToAngle(nextDirection);
                var angleError = currentDirectionAngle - currentRobotAngle;
                currentDirection = nextDirection;
                yield return new Command { Angle = Angle.FromGrad(-angle + angleError), Time = 1 };
                yield return new Command { Move = 50, Time = 1};
            }
        }

        private static int DirectionToAngle(Direction direction)
        {
            if (direction == Direction.Up)
                return -270;
            if (direction == Direction.Right)
                return 0;
            if (direction == Direction.Down)
                return -90;
            if (direction == Direction.Left)
                return -180;
            throw new ArgumentOutOfRangeException();
        }
    }
}
