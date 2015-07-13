using System.Collections.Generic;
using System.Linq;
using CVARC.V2;
using RoboMovies.MapBuilder;

namespace RoboMovies.Bots
{
    class MolagBal : RepairTheStarshipBot
    {
        private Direction lastCommand;

        protected override IEnumerable<MoveAndBuildCommand> FindNextCommands()
        {
            var path = PathSearcher.FindPath(Map, OurCoordinates, OpponentCoordinates);
            var commands = path.Length == 0 ? GoBack() : RobotLocator.GetCommandsByDirection(path.First());
            lastCommand = path.FirstOrDefault();
            return commands;
        }

        private IEnumerable<MoveAndBuildCommand> GoBack()
        {
            return RobotLocator.GetCommandsByDirection(lastCommand.Invert()).Concat(new[] { RMRules.Current.Stand(3) });
        }
    }
}
