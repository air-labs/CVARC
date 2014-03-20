using System.Collections.Generic;
using System.Linq;
using CVARC.Graphics;

namespace CVARC.Basic.Sensors.Positions
{
    public class LightHouseSensor : Sensor<ManyPositionData>
    {
        private readonly List<Robot> robots;

        public LightHouseSensor(Robot robot, World world, DrawerFactory factory) 
            : base(robot, world, factory)
        {
            robots = world.Robots;
        }

        public override ManyPositionData Measure()
        {
            var data = robots.Select(a => new PositionData(a.Body.GetAbsoluteLocation())
                {
                    RobotNumber = a.Number
                }).ToArray();
            return new ManyPositionData(data);
        }
    }
}
