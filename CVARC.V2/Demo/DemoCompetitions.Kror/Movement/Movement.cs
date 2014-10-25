using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;

namespace Demo.KroR
{
    public class Movement : Competitions
    {
        public Movement()
            : base(new MovementLogicPart(), new KroREnginePart(), new MovementManagerPart())
        { }
    }
}
