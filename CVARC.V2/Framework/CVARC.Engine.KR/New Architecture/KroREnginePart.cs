using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class KroREnginePart : EnginePart
    {
        public KroREnginePart()
            : base(()=>new KroREngine(), ()=>new WinformsKeyboard())
        { }
    }
}
