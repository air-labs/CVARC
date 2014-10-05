using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class Environments
    {
        public static string SeedKey = "Seed";
        public static Dictionary<string, Func<IEnvironment>> Available = new Dictionary<string, Func<IEnvironment>>();
        static Environments()
        {
            Available["Tutorial"] = () => new TutorialEnvironment();
            Available["BotDemo"] = () => new BotDemoEnvironment();
        }
    }
}
