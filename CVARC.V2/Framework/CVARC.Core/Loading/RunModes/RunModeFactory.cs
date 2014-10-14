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
                case RunModes.Play: return new LogPlayRunMode();
                case RunModes.Debug: return new DebugRunMode();
            }
            throw new ArgumentException();
        }
    }
}
