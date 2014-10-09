using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class RunModeArguments
    {
        public string Assembly;
        public string Level;
        public string Mode;
        public int Seed;
        public readonly Dictionary<string, string> ControllersInfo = new Dictionary<string, string>();
    }


}
