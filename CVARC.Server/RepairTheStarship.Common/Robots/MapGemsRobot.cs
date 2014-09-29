using RepairTheStarship;
using RepairTheStarship.Sensors;

namespace Gems.Robots
{
    public class MapGemsRobot : BaseGemsRobot
    {
        public MapGemsRobot(SRCompetitions competitions, int number)
            : base(competitions, number)
        {
        }

        public override T GetSensorsData<T>()
        {
            return new PositionSensorsData
                {
                    RobotId = RobotIdSensor.Measure(),
                    Position = PositionSensor.Measure(),
                    MapSensor = MapSensor.Measure()
                } as T;
        }
    }
}