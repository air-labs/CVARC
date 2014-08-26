using RepairTheStarship;
using RepairTheStarship.Sensors;

namespace Gems.Robots
{
    public class GemsRobot : BaseGemsRobot
    {
        public GemsRobot(SRCompetitions competitions, int number)
            : base(competitions, number)
        {
        }

        public override T GetSensorsData<T>()
        {
            return new SensorsData
                {
                    RobotIdSensor = RobotIdSensor.Measure(),
                    LightHouseSensor = PositionSensor.Measure(),
                    MapSensor = MapSensor.Measure()
                } as T;
        }
    }
}
