using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class TutorialControllerFactory : ControllerFactory
    {
        IKeyboardControllerPool pool;

        public override void Initialize(IWorld world)
        {
            base.Initialize(world);
            pool = world.Competitions.Logic.CreateKeyboardControllerPool();
            pool.Initialize(world, world.Competitions.Engine.KeyboardFactory());
        }


        override public IController Create(string controllerId)
        {
            return pool.CreateController(controllerId);
        }
    }
}
