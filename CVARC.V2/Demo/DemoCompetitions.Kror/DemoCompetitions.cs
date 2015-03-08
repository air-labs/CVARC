using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;

namespace Demo.KroR
{
    public class DemoCompetitions : Competitions
    {
		public DemoCompetitions()
            : base(new DemoLogicPartHelper(), new KroREnginePart(), new MovementManagerPart())
        { }
    }
}
