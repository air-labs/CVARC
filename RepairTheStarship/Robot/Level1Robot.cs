using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepairTheStarship.Sensors;

namespace RepairTheStarship.Robot
{
    public class Level1Robot : RTSRobot<Level1SensorData>
    {
        public Level1Robot(string controllerId)
            : base(controllerId)
        { }

    }
}
