using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;
using Demo.Collision;

namespace Demo.KroR
{
    public class Collision : Competitions
    {
        public Collision()
            : base(new CollisionLogicPart(), new KroREnginePart(), new MovementManagerPart())
        { }
    }
}
