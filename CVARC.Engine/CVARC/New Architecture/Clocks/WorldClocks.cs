using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class WorldClocks
    {
        List<ClockdownRecord> clockdown = new List<ClockdownRecord>();

        public double CurrentTime { get; private set; }

        public double GetNextEventTime()
        {
            return clockdown.Select(z => z.ScheduledTime).Where(z => z >= CurrentTime).Min();
        }

        public void SetClockdown(double firstCallTime, ClockdownDelegate clockdownMethod)
        {
            clockdown.Add(new ClockdownRecord { ClockdownMethod = clockdownMethod, LastCallTime = 0, ScheduledTime = firstCallTime });
        }

        public void Tick(double time)
        {
            CurrentTime = time;
            if (clockdown.Count == 0) return;
            while (true)
            {
                var ready = clockdown.Where(z => z.ScheduledTime <= time);
                if (!ready.Any()) return;
                var min = ready.Min(z => z.ScheduledTime);
                var recordToRun = clockdown.Where(z => z.ScheduledTime == min).First();
                clockdown.Remove(recordToRun);
                var clockdownData = new ClockdownData { PreviousCallTime = recordToRun.LastCallTime, ScheduledTime = recordToRun.ScheduledTime, ThisCallTime = time };
                double nextCall;
                recordToRun.ClockdownMethod(clockdownData, out nextCall);
                clockdown.Add(new ClockdownRecord { ClockdownMethod = recordToRun.ClockdownMethod, ScheduledTime = nextCall, LastCallTime = time });
            }
        }

    }
}
