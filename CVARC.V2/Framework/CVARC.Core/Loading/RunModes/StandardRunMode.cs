using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class StandardRunMode : IRunMode
    {
        public Competitions Competitions { get; private set; }
        public Configuration Configuration { get; private set; }
        public double TimeLimit { get; private set; }
    
        public virtual void InitializeCompetitions(Competitions competitions)
        {
            this.Competitions = competitions;
        }

        public void CheckArguments(Configuration arguments)
        {
            this.Configuration = arguments;
        }

        public abstract IController GetController(string controllerId);
    }
}
