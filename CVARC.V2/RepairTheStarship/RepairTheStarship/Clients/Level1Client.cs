using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepairTheStarship
{
    public class Level1Client : RTSClient<Level1SensorData>
    {

        public override string LevelName
        {
            get { return "Level1"; }
        }
    }
}
