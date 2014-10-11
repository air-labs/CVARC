using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class TimerTrigger : Trigger
    {
        Action<double> action;
        double interval;

        public TimerTrigger(Action<double> action, double interval)
        {
            this.action = action;
            this.interval = interval;
        }

        public override TriggerKeep Act(double time)
        {
            action(time);
            ScheduledTime += interval;
            return TriggerKeep.Keep;
        }
    }
}
