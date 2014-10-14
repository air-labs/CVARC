using System.Collections.Generic;
using System.Linq;
using CVARC.V2.SimpleMovement;
using RepairTheStarship.MapBuilder;

namespace RepairTheStarship.Bots
{
    class MolagBal : RepairTheStarshipBot
    {
        private Direction lastCommand;

        protected override IEnumerable<SimpleMovementCommand> FindNextCommands()
        {
            var path = PathSearcher.FindPath(Map, OurCoordinates, OpponentCoordinates);
            var commands = path.Length == 0 ? GoBack() : RobotLocator.GetCommandsByDirection(path.First());
            lastCommand = path.FirstOrDefault();
            return commands;
        }

        private IEnumerable<SimpleMovementCommand> GoBack()
        {
            return RobotLocator.GetCommandsByDirection(lastCommand.Invert()).Concat(new[] { world.CommandHelper.SleepCommand(3) });
        }
    }
}
