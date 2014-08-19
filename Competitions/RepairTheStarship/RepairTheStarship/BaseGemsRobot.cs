using CVARC.Basic;
using CVARC.Basic.Sensors;
using CVARC.Basic.Sensors.Positions;
using RepairTheStarship.Sensors;

namespace RepairTheStarship
{
    public abstract class BaseGemsRobot : Robot
    {
        protected RobotIdSensor RobotIdSensor;
        protected MapSensor MapSensor;
        protected LightHouseSensor LightHouseSensor;
        protected DestinationMapSensor DestinationMapSensor;

        protected BaseGemsRobot(Competitions competitions, int number) : base(competitions, number)
        {
        }

        public override void Init()
        {
            RobotIdSensor = new RobotIdSensor(this);
            MapSensor = new MapSensor(this);
            LightHouseSensor = new LightHouseSensor(this);
            DestinationMapSensor = new DestinationMapSensor(this);
        }
    }
}