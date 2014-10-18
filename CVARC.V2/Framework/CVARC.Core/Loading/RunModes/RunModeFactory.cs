using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class RunModeFactory
    {
        public static IRunMode Create(RunModes mode)
        {
            switch (mode)
            {
                case RunModes.Tutorial: return new TutorialRunMode();
                case RunModes.BotDemo: return new BotDemoRunMode();
                case RunModes.Tournament: return new TournamentRunMode();
            }
            throw new ArgumentException();
        }
    }
}
