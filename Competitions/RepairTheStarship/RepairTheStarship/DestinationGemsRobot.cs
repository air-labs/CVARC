using RepairTheStarship.Sensors;

namespace RepairTheStarship
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
                LightHouseSensor = LightHouseSensor.Measure(),
                MapSensor = DestinationMapSensor.Measure()
            } as T;
        }
    }
}