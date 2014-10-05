using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public interface IRunMode
    {
        void Initialize(RunModeArguments arguments, Competitions competitions);
        ISceneSettings GetSceneSettings();
        void PrepareControllers(string[] allControllersId);
        IController GetController(string controllerId);
    }

}