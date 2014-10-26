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

        public virtual void Initialize(IWorld world, Configuration configuration, Competitions competitions)
        {
            this.Competitions = competitions;
            this.Configuration = configuration;
        }

        public abstract IController GetController(string controllerId);
    }
}
