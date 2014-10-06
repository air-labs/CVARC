using System.Collections.Generic;
using System.Linq;
using CVARC.V2.SimpleMovement;
using RepairTheStarship.MapBuilder;

namespace RepairTheStarship.Bots
{
    public class Vaermina : RepairTheStarshipBot
    {
        protected override IEnumerable<SimpleMovementCommand> FindNextCommands()
        {
            var path = PathSearcher.FindPath(Map, OurCoordinates, OpponentCoordinates);
            return path.Length == 0 || path.Length == 1 ? new SimpleMovementCommand[0] : RobotLocator.GetCommandsByDirection(path.First());
        }
    }
}
