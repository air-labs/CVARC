using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;

namespace RepairTheStarship.KroR
{
    public class Level1 : Competitions
    {
        public Level1()
			: base(new RTSLogicPartHepler<FullMapSensorData>(), new KroREnginePart(), new RTSManagerPart())
        { }
    }

    public class Level2 : Competitions
    {
        public Level2()
			: base(new RTSLogicPartHepler<FullMapSensorData>(), new KroREnginePart(), new RTSManagerPart())
        { }
    }

    public class Level3 : Competitions
    {
        public Level3()
			: base(new RTSLogicPartHepler<LimitedMapSensorData>(), new KroREnginePart(), new RTSManagerPart())
        { }
    }
   

}
