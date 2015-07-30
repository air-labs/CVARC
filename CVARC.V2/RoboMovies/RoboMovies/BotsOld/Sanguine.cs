using System;
using System.Collections.Generic;
using CVARC.V2;
using RoboMovies.MapBuilder;

namespace RoboMovies.Bots
{
    public class Sanguine : RepairTheStarshipBot
    {
        private readonly Random random = new Random();
        private Direction[] currentPath = new Direction[0];
        private int currentCommand;

        protected override IEnumerable<MoveAndBuildCommand> FindNextCommands()
        {
            while (currentCommand >= currentPath.Length)
            {
                var x = random.Next(1, 7);
                var y = random.Next(1, 5);
                currentPath = PathSearcher.FindPath(Map, OurCoordinates, new Point(x, y));
                currentCommand = 0;
            }
            return RobotLocator.GetCommandsByDirection(currentPath[currentCommand++]);
        }
    }
}
