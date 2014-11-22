using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public class TutorialRunMode : StandardRunMode
    {
        IKeyboardControllerPool pool;

        override public IController GetController(string controllerId)
        {
            return pool.CreateController(controllerId);
        }

        override public void Initialize(IWorld world, Configuration configuration, Competitions competitions)
        {
            base.Initialize(world, configuration, competitions);
            this.pool = competitions.Logic.CreateKeyboardControllerPool();
            pool.Initialize(world, competitions.Engine.KeyboardFactory());
        }

    }
}
