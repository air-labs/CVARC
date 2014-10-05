using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class StandardEnvironment : IEnvironment
    {
        protected Competitions competitions { get; private set; }
        protected Dictionary<string, string> commandLineArgs { get; private set; }

        public virtual void Initialize(Dictionary<string, string> commandLineArguments, Competitions competitions)
        {
            this.commandLineArgs = commandLineArguments;
            this.competitions = competitions;
        }

        public Basic.ISceneSettings GetSceneSettings()
        {
            int seed = 0;
            if (commandLineArgs.ContainsKey(Environments.SeedKey))
            {
                try
                {
                    seed = int.Parse(commandLineArgs[Environments.SeedKey]);
                }
                catch
                {
                    throw new Exception("The key " + Environments.SeedKey + " must have an integer value");
                }
            }
            return competitions.Logic.MapGenerator(seed);
        }

        public abstract void PrepareControllers(string[] allControllersId);

        public abstract IController GetController(string controllerId);
    }
}
