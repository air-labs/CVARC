using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;


namespace Demo
{
    public class DemoCompetitions : Competitions
    {
		public DemoCompetitions()
            : base(new DemoLogicPartHelper(), new KroREnginePart(), new MovementManagerPart())
        { }
    }

	public class DWMCompetitions : Competitions
	{
		public DWMCompetitions()
			: base(new DWMLogicPartHelper(), new KroREnginePart(), new MovementManagerPart())
		{ }
	}
}
