using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class StandardRunMode : IRunMode
    {
        protected Competitions Competitions { get; private set; }
        public Configuration Arguments { get; private set; }
        public double TimeLimit { get; private set; }
    
        public virtual void InitializeCompetitions(Competitions competitions)
        {
            this.Competitions = competitions;
        }

        public void CheckArguments(Configuration arguments)
        {
            this.Arguments = arguments;
        }

        public Basic.ISceneSettings GetSceneSettings()
        {
            return Competitions.Logic.MapGenerator(Arguments.Seed);
        }

        public abstract void PrepareControllers(string[] allControllersId);

        public abstract IController GetController(string controllerId);
    }
}
