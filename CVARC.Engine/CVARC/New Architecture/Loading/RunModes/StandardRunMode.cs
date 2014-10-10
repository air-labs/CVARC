using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class StandardRunMode : IRunMode
    {
        protected Competitions Competitions { get; private set; }
        public RunModeArguments Arguments { get; private set; }
        public double TimeLimit { get; private set; }
        RunModeArguments IRunMode.Arguments { get { return Arguments; } }

        public virtual void Initialize(RunModeArguments runModeArguments, Competitions competitions)
        {
            this.Arguments = runModeArguments;
            this.Competitions = competitions;
        }

        public Basic.ISceneSettings GetSceneSettings()
        {
            return Competitions.Logic.MapGenerator(Arguments.Seed);
        }

        public abstract void PrepareControllers(string[] allControllersId);

        public abstract IController GetController(string controllerId);
    }
}
