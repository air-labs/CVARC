using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;

namespace Demo.KroR
{
    public class Interaction : Competitions
    {
        public Interaction()
            : base(new InteractionLogicPartHelper(), new KroREnginePart(), new InteractionManagerPart())
        { }
    }
}
