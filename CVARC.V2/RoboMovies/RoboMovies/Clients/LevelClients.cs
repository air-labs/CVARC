using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboMovies
{
    public class Level1Client : RMClient<FullMapSensorData>
    {
        public override string LevelName
        {
            get { return "Level1"; }
        }
    }

	public class Level2Client : RMClient<FullMapSensorData>
    {
        public override string LevelName
        {
            get { return "Level2"; }
        }
    }

    public class Level3Client : RMClient<LimitedMapSensorData>
    {
        public override string LevelName
        {
            get { return "Level3"; }
        }
    }
}
