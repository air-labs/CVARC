using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class WorldClocks
    {
        List<Trigger> triggers = new List<Trigger>();

        public double CurrentTime { get; private set; }

        public double GetNextEventTime()
        {
            return triggers.Select(z => z.ScheduledTime).Where(z => z >= CurrentTime).Min();
        }

        public void AddOneTimeTrigger(double scheduledTime, Action action)
        {
            triggers.Add(new Trigger { ScheduledTime = scheduledTime, Action = action });
        }

        public void AddRenewableTrigger(double firstCallTime, RenewableTriggerDelegate clockdownMethod)
        {
            RenewTrigger(0, firstCallTime, clockdownMethod);
        }

        void RenewTrigger(double lastCall, double scheduledTime, RenewableTriggerDelegate clockdownMethod)
        {
            triggers.Add(new Trigger
                {
                    ScheduledTime = scheduledTime,
                    Action = () => RunRenewvableTrigger(lastCall, scheduledTime, clockdownMethod)
                });
        }


        void RunRenewvableTrigger(double lastCall, double scheduledTime, RenewableTriggerDelegate clockdownMethod)
        {
            var clockdownData = new RenewableTriggerData { PreviousCallTime = lastCall, ScheduledTime = scheduledTime, ThisCallTime = CurrentTime };
            double nextCall;
            clockdownMethod(clockdownData, out nextCall);
            RenewTrigger(clockdownData.ThisCallTime,nextCall,clockdownMethod);
        }

        
        

        public void Tick(double time)
        {
            CurrentTime = time;
            if (triggers.Count == 0) return;
            while (true)
            {
                var ready = triggers.Where(z => z.ScheduledTime <= time);
                if (!ready.Any()) return;
                var min = ready.Min(z => z.ScheduledTime);
                var recordToRun = triggers.Where(z => z.ScheduledTime == min).First();
                triggers.Remove(recordToRun);
                recordToRun.Action();
            }
        }

    }
}
