using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class KREnginePart : EnginePart
    {
        public KREnginePart()
            : base(new KRPhysical(), new WinformsKeyboard())
        { }
    }
}
