using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    [Serializable]
    public partial class Configuration
    {
        public string Assembly { get; set; }
        public string Level { get; set; }
        public string Mode { get; set; }
        public int Seed { get; set; }
        public double? TimeLimit { get; set; }
        public bool EnableLog { get; set; }
        public string LogFile { get; set; }
        public bool SpeedUp { get; set; }
        public readonly Dictionary<string, string> Controllers = new Dictionary<string, string>();
    }


}
