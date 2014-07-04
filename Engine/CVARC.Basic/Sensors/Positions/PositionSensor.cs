

namespace CVARC.Basic.Sensors.Positions
{
    public class PositionSensor : Sensor<PositionData>
    {
        public PositionSensor(Robot robot) 
            : base(robot)
        {
        }

        public override PositionData Measure()
        {
            return new PositionData(Robot.GetAbsoluteLocation()) { RobotNumber = Robot.Number };
        }
    }
}
