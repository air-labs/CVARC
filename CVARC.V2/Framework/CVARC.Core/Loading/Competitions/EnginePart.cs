using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class EnginePart
    {
        public readonly Func<IEngine> EngineFactory;
        public readonly Func<IKeyboard> KeyboardFactory;
        public EnginePart(Func<IEngine> engineFactory, Func<IKeyboard> keyboardFactory)
        {
            this.EngineFactory = engineFactory;
            this.KeyboardFactory = keyboardFactory;
        }
    }
}
