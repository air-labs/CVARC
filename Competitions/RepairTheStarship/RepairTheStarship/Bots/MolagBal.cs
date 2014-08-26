using System.Collections.Generic;
using System.Linq;
using CVARC.Basic.Controllers;
using MapHelper;

namespace Gems.Bots
{
    class MolagBal : RepairTheStarshipBot
    {
        private Direction lastCommand;

        protected override IEnumerable<Command> FindNextCommands()
        {
            var path = PathSearcher.FindPath(Map, OurCoordinates, OpponentCoordinates);
            var commands = path.Length == 0 ? GoBack() : RobotLocator.GetCommandsByDirection(path.First());
            lastCommand = path.FirstOrDefault();
            return commands;
        }

        private IEnumerable<Command> GoBack()
        {
            return RobotLocator.GetCommandsByDirection(lastCommand.Invert()).Concat(new[] {Command.Sleep(3)});
        }
    }
}
