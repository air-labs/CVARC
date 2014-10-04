using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public abstract class Environment
    {
        public abstract ISceneSettings GetSceneSettings();
        public abstract IController GetController(string controllerId);
        public readonly Competitions Competitions;
        public Environment(Competitions competitions)
        {
            this.Competitions = competitions;
        }
    }

}