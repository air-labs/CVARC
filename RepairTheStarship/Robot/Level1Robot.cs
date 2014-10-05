using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepairTheStarship.Sensors;

namespace RepairTheStarship.Robot
{
    public class Level1Robot : RTSRobot<SensorsData>
    {
        public Level1Robot(string controllerId)
            : base(controllerId)
        { }

        protected override SensorsData GetSensorsData()
        {
            return new SensorsData();
        }
    }
}
