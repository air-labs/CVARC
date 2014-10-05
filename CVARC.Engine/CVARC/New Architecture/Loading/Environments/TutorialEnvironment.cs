using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public class TutorialEnvironment : IEnvironment
    {
        IKeyboardControllerPool pool;
        Competitions competitions;
        Dictionary<string, string> commandLineArgs;

        public ISceneSettings GetSceneSettings()
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

        public IController GetController(string controllerId)
        {
            return pool.CreateController(controllerId);
        }

        public void Initialize(Dictionary<string,string> commandLineArgs, Competitions competitions)
        {
            this.commandLineArgs = commandLineArgs;
            this.competitions = competitions;
            this.pool = competitions.Logic.KeyboardControllerPoolFactory(competitions.Engine.Keyboard);
        }

        public void PrepareControllers(string[] allControllersId)
        {
            
        }
    }
}
