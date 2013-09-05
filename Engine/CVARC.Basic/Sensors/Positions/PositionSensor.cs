using CVARC.Graphics;

namespace CVARC.Basic.Sensors
{
    public class PositionSensor: ISensor
    {
        private Robot _robot;
        public void Init(Robot robot, World wrld, DrawerFactory factory)
        {
            _robot = robot;
        }

        public ISensorData Measure()
        {
            return new PositionData{Position = _robot.Body.GetAbsoluteLocation(), RobotNumber = _robot.Number};
        }
    }
}
