using CVARC.Basic;
using CVARC.Basic.Sensors;
using CVARC.Basic.Sensors.Positions;
using RepairTheStarship.Sensors;

namespace Gems.Robots
{
    public abstract class BaseGemsRobot : Robot
    {
        protected RobotIdSensor RobotIdSensor;
        protected MapSensor MapSensor;
        protected LightHouseSensor PositionSensor;
        protected DestinationMapSensor DestinationMapSensor;
        protected RobotCamera RobotCamera;

        protected BaseGemsRobot(Competitions competitions, int number) : base(competitions, number)
        {
        }

        public override void Init()
        {
            RobotIdSensor = new RobotIdSensor(this);
            MapSensor = new MapSensor(this);
            PositionSensor = new LightHouseSensor(this);
            DestinationMapSensor = new DestinationMapSensor(this);
            RobotCamera = new RobotCamera(this);
        }
    }
}