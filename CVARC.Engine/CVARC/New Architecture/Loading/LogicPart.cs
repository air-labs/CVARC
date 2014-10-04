using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class LogicPart
    {
        public readonly IWorld World;
        public readonly Func<IKeyboard,IKeyboardControllerPool> KeyboardControllerPoolFactory;
        public LogicPart(IWorld world, Func<IKeyboard, IKeyboardControllerPool> keyboardControllerPoolFactory)
        {
            this.World = world;
            this.KeyboardControllerPoolFactory = keyboardControllerPoolFactory;
        }
    }
}
