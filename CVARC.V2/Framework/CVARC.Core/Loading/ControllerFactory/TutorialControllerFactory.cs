using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class TutorialControllerFactory : ControllerFactory
    {


        override public IController Create(string controllerId)
        {
            var controller = World.Competitions.Logic.Actors[controllerId].CreateKeyboardController();
            var actor = World.Actors.Where(z => z.ControllerId == controllerId).First();
            actor.Rules.DefineKeyboardControl(controller, controllerId);
            return controller;
        }
    }
}
