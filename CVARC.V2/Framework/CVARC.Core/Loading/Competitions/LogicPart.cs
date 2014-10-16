using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public class LogicPart
    {
        public readonly IWorld World;
        public readonly Func<IKeyboardControllerPool> KeyboardControllerPoolFactory;
        public readonly Dictionary<string, Func<IController>> Bots = new Dictionary<string, Func<IController>>();
        public readonly double TimeLimit;
        public LogicPart(
            IWorld world, 
            Func<IKeyboardControllerPool> keyboardControllerPoolFactory,
            double TimeLimit
            )
        {
            this.World = world;
            this.KeyboardControllerPoolFactory = keyboardControllerPoolFactory;
            this.TimeLimit = TimeLimit;
        }
    }
}
