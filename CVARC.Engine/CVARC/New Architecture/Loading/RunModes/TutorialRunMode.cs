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

        override public void InitializeCompetitions(Competitions competitions)
        {
            base.InitializeCompetitions(competitions);
            this.pool = competitions.Logic.KeyboardControllerPoolFactory();
            pool.Initialize(competitions.Logic.World, competitions.Engine.Keyboard);
        }

        override public void PrepareControllers(string[] allControllersId)
        {
            
        }
    }
}
