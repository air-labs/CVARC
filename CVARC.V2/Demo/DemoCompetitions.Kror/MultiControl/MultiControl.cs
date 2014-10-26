using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;
using Demo;

namespace Demo.KroR
{
    public class MultiControl : Competitions
    {
        public MultiControl()
            : base(new MultiControlLogicPart(), new KroREnginePart(), new MovementManagerPart())
        { }
    }
}
