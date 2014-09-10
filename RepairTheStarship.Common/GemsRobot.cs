using CVARC.Basic;
using CVARC.Basic.Sensors;
using CVARC.Basic.Sensors.Positions;
using RepairTheStarship.Sensors;

namespace RepairTheStarship
{
    public class GemsRobot : Robot
    {
        private RobotIdSensor robotIdSensor;
        private MapSensor mapSensor;
        private LightHouseSensor lightHouseSensor;

        public GemsRobot(SRCompetitions competitions, int number)
            : base(competitions, number)
        {
        }

        public override T GetSensorsData<T>()
        {
            return new SensorsData
                {
                    RobotIdSensor = robotIdSensor.Measure(),
                    LightHouseSensor = lightHouseSensor.Measure(),
                    MapSensor = mapSensor.Measure()
                } as T;
        }

        public override void Init()
        {
            robotIdSensor = new RobotIdSensor(this);
            mapSensor = new MapSensor(this);
            lightHouseSensor = new LightHouseSensor(this);
        }
    }
}
