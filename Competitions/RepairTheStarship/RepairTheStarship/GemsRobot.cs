using RepairTheStarship.Sensors;

namespace RepairTheStarship
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
                    LightHouseSensor = LightHouseSensor.Measure(),
                    MapSensor = MapSensor.Measure()
                } as T;
        }
    }
}
