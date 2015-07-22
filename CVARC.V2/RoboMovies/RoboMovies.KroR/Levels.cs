using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;

namespace RoboMovies.KroR
{
    public class Level1 : Competitions
    {
        public Level1()
			: base(new RMLogicPartHelper(), new KroREnginePart(), new RMManagerPart())
        { }
    }

    public class Level2 : Competitions
    {
        public Level2()
			: base(new RMLogicPartHelper(), new KroREnginePart(), new RMManagerPart())
        { }
    }

    public class Level3 : Competitions
    {
        public Level3()
			: base(new RMLogicPartHelper(), new KroREnginePart(), new RMManagerPart())
        { }
    }
   

}
