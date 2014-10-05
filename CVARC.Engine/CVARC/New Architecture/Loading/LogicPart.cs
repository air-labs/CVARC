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
        public readonly Func<int, ISceneSettings> MapGenerator;
        public LogicPart(
            IWorld world, 
            Func<IKeyboardControllerPool> keyboardControllerPoolFactory,
            Func<int,ISceneSettings> mapGenerator
            )
        {
            this.World = world;
            this.KeyboardControllerPoolFactory = keyboardControllerPoolFactory;
            this.MapGenerator = mapGenerator;
        }
    }
}
