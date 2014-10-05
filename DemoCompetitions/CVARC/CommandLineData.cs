using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVARC
{
    public class CommandLineData
    {
        public readonly List<string> Unnamed = new List<string>();
        public readonly Dictionary<string, string> Named = new Dictionary<string, string>();
    }
}
