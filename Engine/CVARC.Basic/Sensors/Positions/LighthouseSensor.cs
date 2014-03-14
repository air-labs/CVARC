using System.Collections.Generic;
using System.Linq;
using CVARC.Graphics;

namespace CVARC.Basic.Sensors
{
    public class LightHouseSensor : ISensor<ManyPositionData>
    {
        private List<Robot> robots;

        public void Init(Robot robot, World wrld, DrawerFactory factory)
        {
            robots = wrld.Robots;
        }

        public ManyPositionData Measure()
        {
            var data = robots.Select(a => new PositionData(a.Body.GetAbsoluteLocation())
                {
                    RobotNumber = a.Number
                }).ToArray();
            return new ManyPositionData(data);
        }
    }
}
