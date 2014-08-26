using RepairTheStarship;
using RepairTheStarship.Sensors;

namespace Gems.Robots
{
    public class DestinationGemsRobot : GemsRobot
    {
        public DestinationGemsRobot(SRCompetitions competitions, int number) 
            : base(competitions, number)
        {
        }

        public override T GetSensorsData<T>()
        {
            return new SensorsData
            {
                RobotIdSensor = RobotIdSensor.Measure(),
                LightHouseSensor = PositionSensor.Measure(),
                MapSensor = DestinationMapSensor.Measure()
            } as T;
        }
    }
}