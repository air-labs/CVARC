using CVARC.Basic;
using RepairTheStarship.Robots;
using RepairTheStarship;

namespace RepairTheStarship.Levels
{
    public abstract class BaseLevel : SRCompetitions
    {
        public override Robot CreateBot(int robotNumber)
        {
            return new MapGemsRobot(this, robotNumber);
        }
    }
}