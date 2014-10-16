using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public abstract class LogicPart
    {
        public readonly IWorld World;
        public readonly Func<IKeyboardControllerPool> KeyboardControllerPoolFactory;
        public readonly Dictionary<string, Func<IController>> Bots = new Dictionary<string, Func<IController>>();
        public LogicPart(
            IWorld world, 
            Func<IKeyboardControllerPool> keyboardControllerPoolFactory
            )
        {
            this.World = world;
            this.KeyboardControllerPoolFactory = keyboardControllerPoolFactory;
        }

        public abstract Settings GetDefaultSettings();
    }
}
