using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public delegate void ClockdownTriggerAction(ClockdownTrigger trigger, out double NextScheduledTime);

    public class ClockdownTrigger
    {
        public ClockdownTriggerAction ScheduledAction;
        public double CurrentlyScheduledTime { get; private set; }
        public double PreviousCallTime { get; private set; }
        public double ThisCallTime { get; private set; }
        public double DeltaTime { get { return ThisCallTime - PreviousCallTime; } }
        public void Tick(double time)
        {
            if (time > CurrentlyScheduledTime)
            {
                ThisCallTime = time;
                double nextTime;
                ScheduledAction(this, out nextTime);
                CurrentlyScheduledTime = nextTime;
                PreviousCallTime = time;
            }
        }
        public ClockdownTrigger(ClockdownTriggerAction action, double firstLaunchTime=-1)
        {
            this.ScheduledAction = action;
        }
    }
}
