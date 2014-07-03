using CVARC.Graphics;

namespace CVARC.Basic.Sensors.Positions
{
    public class PositionSensor : Sensor<PositionData>
    {
        public PositionSensor(Robot robot, World world) 
            : base(robot, world)
        {
        }

        public override PositionData Measure()
        {
            return new PositionData(Robot.GetAbsoluteLocation()) { RobotNumber = Robot.Number };
        }
    }
}
