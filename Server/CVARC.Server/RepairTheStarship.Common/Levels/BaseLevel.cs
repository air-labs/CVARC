using CVARC.Basic;
using Gems.Robots;
using RepairTheStarship;

namespace Gems.Levels
{
    public abstract class BaseLevel : SRCompetitions
    {
        public override Robot CreateBot(int robotNumber)
        {
            return new MapGemsRobot(this, robotNumber);
        }
    }
}