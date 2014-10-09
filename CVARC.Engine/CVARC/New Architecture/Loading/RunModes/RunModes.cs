using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class RunModes
    {
        public static string SeedKey = "Seed";
        public static Dictionary<string, Func<IRunMode>> Available = new Dictionary<string, Func<IRunMode>>();
        static RunModes()
        {
            Available["Tutorial"] = () => new TutorialRunMode();
            Available["BotDemo"] = () => new BotDemoRunMode();
        }
    }
}
