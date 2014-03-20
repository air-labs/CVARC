using CVARC.Graphics;

namespace CVARC.Basic.Sensors.Positions
{
    public class PositionSensor : Sensor<PositionData>
    {
        public PositionSensor(Robot robot, World world, DrawerFactory factory) 
            : base(robot, world, factory)
        {
        }

        public override PositionData Measure()
        {
            return new PositionData(Robot.Body.GetAbsoluteLocation()) { RobotNumber = Robot.Number };
        }
    }
}
