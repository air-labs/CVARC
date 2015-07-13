using System.Collections.Generic;
using System.Linq;
using CVARC.V2;
using RoboMovies.MapBuilder;

namespace RoboMovies.Bots
{
    public class Vaermina : RepairTheStarshipBot
    {
        protected override IEnumerable<MoveAndBuildCommand> FindNextCommands()
        {
            var path = PathSearcher.FindPath(Map, OurCoordinates, OpponentCoordinates);
            return path.Length == 0 || path.Length == 1 ? new MoveAndBuildCommand[0] : RobotLocator.GetCommandsByDirection(path.First());
        }
    }
}
