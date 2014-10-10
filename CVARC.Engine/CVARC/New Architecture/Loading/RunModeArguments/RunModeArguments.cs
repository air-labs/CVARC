using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public partial class RunModeArguments
    {
        public string Assembly { get; private set; }
        public string Level { get; private set; }
        public string Mode { get; private set; }
        public int Seed { get; private set; }
        public double? TimeLimit { get; private set; }
        public bool EnableLog { get; private set; }
        public string LogFile { get; private set; }
        public bool SpeedUp { get; private set; }
        public readonly Dictionary<string, string> Controllers = new Dictionary<string, string>();
    }


}
