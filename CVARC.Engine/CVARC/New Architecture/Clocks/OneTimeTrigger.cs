using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class OneTimeTrigger : Trigger
    {
        Action action;

        public OneTimeTrigger(double time, Action action)
        {
            this.action = action;
            this.ScheduledTime = time;
        }

        public override TriggerKeep  Act(double time)
        {
            action();
            return TriggerKeep.Remove;
        }
    }
}
