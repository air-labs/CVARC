using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class Trigger
    {
        public double ScheduledTime { get; set; }
        public Action Action { get; set; }
    }
}
