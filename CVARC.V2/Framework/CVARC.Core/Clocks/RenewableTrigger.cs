using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class RenewableTrigger : Trigger
    {
        public double LastCall { get; private set; }
        public double ThisCall { get; private set; }
        public abstract void Act(out double nextTime);
        public override TriggerKeep Act(double time)
        {
            ThisCall = time;
            double next;
            Act(out next);
            LastCall = ThisCall;
            ScheduledTime = next;
            return TriggerKeep.Keep;
        }
    }
}
