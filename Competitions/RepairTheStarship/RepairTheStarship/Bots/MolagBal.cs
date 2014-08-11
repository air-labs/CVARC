using System.Collections.Generic;
using System.Linq;
using CVARC.Basic.Controllers;
using MapHelper;

namespace Gems.Bots
{
    class MolagBal : RepairTheStarshipBot
    {
        protected override IEnumerable<Command> FindNextCommands()
        {
            var path = PathSearcher.FindPath(Map, OurCoordinates, OpponentCoordinates);
            return path.Length == 0 ? new Command[0] : RobotLocator.GetCommandsByDirection(path.First());
        }
    }
}
