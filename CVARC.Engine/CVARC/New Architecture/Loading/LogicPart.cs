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
        public readonly Dictionary<string, Func<string,IController>> Bots = new Dictionary<string, Func<string,IController>>();
        public LogicPart(IWorld world, Func<IKeyboard, IKeyboardControllerPool> keyboardControllerPoolFactory)
        {
            this.World = world;
            this.KeyboardControllerPoolFactory = keyboardControllerPoolFactory;
        }
    }
}
