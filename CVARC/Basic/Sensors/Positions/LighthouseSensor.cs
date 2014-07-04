using System.Collections.Generic;
using System.Linq;

namespace CVARC.Basic.Sensors.Positions
{
    public class LightHouseSensor : Sensor<ManyPositionData>
    {
        private readonly List<Robot> robots;

        public LightHouseSensor(Robot robot) 
            : base(robot)
        {
            robots = World.Robots;
        }

        public override ManyPositionData Measure()
        {
            var data = robots.Select(a => new PositionData(a.GetAbsoluteLocation())
                {
                    RobotNumber = a.Number
                }).ToArray();
            return new ManyPositionData(data);
        }
    }
}
