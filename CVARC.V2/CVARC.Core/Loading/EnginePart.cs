using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class EnginePart
    {
        public readonly IEngine Engine;
        public readonly IKeyboard Keyboard;
        public EnginePart(IEngine engine, IKeyboard keyboard)
        {
            this.Engine = engine;
            this.Keyboard = keyboard;
        }
    }
}
