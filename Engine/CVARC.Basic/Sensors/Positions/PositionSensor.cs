using CVARC.Graphics;

namespace CVARC.Basic.Sensors
{
    public class PositionSensor : ISensor<PositionData>
    {
        private Robot _robot;

        public void Init(Robot robot, World wrld, DrawerFactory factory)
        {
            _robot = robot;
        }

        public PositionData Measure()
        {
            return new PositionData(_robot.Body.GetAbsoluteLocation()) {RobotNumber = _robot.Number};
        }
    }
}
