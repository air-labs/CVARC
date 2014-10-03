using System.Linq;

namespace CVARC.Basic.Sensors.Positions
{
    public class GripSensor : Sensor<GripData>
    {
        public GripSensor(Robot robot)
            : base(robot)
        {
        }

        public override GripData Measure()
        {
            return new GripData
                {
                    HasGrippedDetail = Robot.Competitions.Engine.GetChilds(Robot.Name).Any()
                };
        }
    }
}