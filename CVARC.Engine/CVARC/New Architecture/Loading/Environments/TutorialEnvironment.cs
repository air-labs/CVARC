using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public class TutorialEnvironment : StandardEnvironment
    {
        IKeyboardControllerPool pool;

        override public IController GetController(string controllerId)
        {
            return pool.CreateController(controllerId);
        }

        override public void Initialize(Dictionary<string,string> commandLineArgs, Competitions competitions)
        {
            base.Initialize(commandLineArgs, competitions);
            this.pool = competitions.Logic.KeyboardControllerPoolFactory(competitions.Engine.Keyboard);
        }

        override public void PrepareControllers(string[] allControllersId)
        {
            
        }
    }
}
