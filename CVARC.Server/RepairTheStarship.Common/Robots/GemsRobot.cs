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
            return new BaseSensorData
                {
                    RobotId = RobotIdSensor.Measure(),
                    Position = PositionSensor.Measure(),
                    DetailsInfo = GripSensor.Measure()
                } as T;
        }
    }
}
