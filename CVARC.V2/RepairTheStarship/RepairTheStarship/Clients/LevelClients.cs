using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepairTheStarship
{
    public class Level1Client : RTSClient<FullMapSensorData>
    {
        public override string LevelName
        {
            get { return "Level1"; }
        }
    }

	public class Level2Client : RTSClient<FullMapSensorData>
    {
        public override string LevelName
        {
            get { return "Level2"; }
        }
    }

    public class Level3Client : RTSClient<LimitedMapSensorData>
    {
        public override string LevelName
        {
            get { return "Level3"; }
        }
    }
}
