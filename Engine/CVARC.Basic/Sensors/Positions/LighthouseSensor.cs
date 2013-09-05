using System.Collections.Generic;
using System.Linq;
using CVARC.Graphics;

namespace CVARC.Basic.Sensors
{
    public class LightHouseSensor : ISensor
    {
        private List<Robot> _robots;
        public void Init(Robot robot, World wrld, DrawerFactory factory)
        {
            _robots = wrld.Robots;
        }

        public ISensorData Measure()
        {
            var data = _robots.Select(a => new PositionData {Position = a.Body.GetAbsoluteLocation(), RobotNumber = a.Number}).ToList();
            return new ManyPositionData(data);
        }
    }
}
