using RepairTheStarship;
using RepairTheStarship.Sensors;

namespace RepairTheStarship.Robots
{
    public class DestinationGemsRobot : GemsRobot
    {
        public DestinationGemsRobot(SRCompetitions competitions, int number) 
            : base(competitions, number)
        {
        }

        public override T GetSensorsData<T>()
        {
            return new PositionSensorsData
            {
                RobotId = RobotIdSensor.Measure(),
                Position = PositionSensor.Measure(),
                MapSensor = DestinationMapSensor.Measure(),
                DetailsInfo = GripSensor.Measure()
            } as T;
        }
    }
}