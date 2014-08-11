using System.Collections.Generic;
using System.Linq;
using CVARC.Basic.Controllers;
using MapHelper;

namespace Gems.Bots
{
    public class Vaermina : RepairTheStarshipBot
    {
        protected override IEnumerable<Command> FindNextCommands()
        {
            var path = PathSearcher.FindPath(Map, OurCoordinates, OpponentCoordinates);
            return path.Length == 0 || path.Length == 1 ? new Command[0] : RobotLocator.GetCommandsByDirection(path.First());
        }
    }
}
