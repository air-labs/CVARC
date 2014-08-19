using CVARC.Basic;
using RepairTheStarship;

namespace Gems.Levels
{
    public abstract class BaseLevel : SRCompetitions
    {
        public override Robot CreateBot(int robotNumber)
        {
            return new GemsRobot(this, robotNumber);
        }
    }
}