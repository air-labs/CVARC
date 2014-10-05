using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class StandardRunMode : IRunMode
    {
        protected Competitions Competitions { get; private set; }
        protected RunModeArguments RunModeArguments { get; private set; }

        public virtual void Initialize(RunModeArguments runModeArguments, Competitions competitions)
        {
            this.RunModeArguments = runModeArguments;
            this.Competitions = competitions;
        }

        public Basic.ISceneSettings GetSceneSettings()
        {
            return Competitions.Logic.MapGenerator(RunModeArguments.Seed);
        }

        public abstract void PrepareControllers(string[] allControllersId);

        public abstract IController GetController(string controllerId);
    }
}
