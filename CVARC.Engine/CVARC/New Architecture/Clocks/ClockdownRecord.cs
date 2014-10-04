using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class ClockdownRecord
    {
        public double ScheduledTime { get; set; }
        public double LastCallTime { get; set; }
        public ClockdownDelegate ClockdownMethod { get; set; }
    }
}
