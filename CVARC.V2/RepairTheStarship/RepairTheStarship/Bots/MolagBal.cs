using System.Collections.Generic;
using System.Linq;
using CVARC.V2;
using RepairTheStarship.MapBuilder;

namespace RepairTheStarship.Bots
{
    class MolagBal : RepairTheStarshipBot
    {
        private Direction lastCommand;

        protected override IEnumerable<MoveAndGripCommand> FindNextCommands()
        {
            var path = PathSearcher.FindPath(Map, OurCoordinates, OpponentCoordinates);
            var commands = path.Length == 0 ? GoBack() : RobotLocator.GetCommandsByDirection(path.First());
            lastCommand = path.FirstOrDefault();
            return commands;
        }

        private IEnumerable<MoveAndGripCommand> GoBack()
        {
            return RobotLocator.GetCommandsByDirection(lastCommand.Invert()).Concat(new[] { RTSRules.Current.Stand(3) });
        }
    }
}
