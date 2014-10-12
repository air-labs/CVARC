using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public enum TriggerKeep
    {
        Keep,
        Remove
    }

    public abstract class Trigger
    {
        public double ScheduledTime { get; protected set; }

        public abstract TriggerKeep  Act(double time);
    }
}
