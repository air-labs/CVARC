using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class WorldClocks
    {
        List<Trigger> triggers = new List<Trigger>();

        public double TimeLimit { get; internal set; }

        public double CurrentTime { get; private set; }

        public double GetNextEventTime()
        {
            return triggers.Select(z => z.ScheduledTime).Where(z => z >= CurrentTime).Min();
        }

        public void AddTrigger(Trigger trigger)
        {
            triggers.Add(trigger);
        }


        public event Action Ticked;
        
		const double TimeDelta = 1e-5;

        public void Tick(double time)
        {
            CurrentTime = time;
            if (triggers.Count == 0) return;

            while (true)
            {
                var ready = triggers.Where(z => z.ScheduledTime <= time+TimeDelta);
                if (!ready.Any()) break;
                var min = ready.Min(z => z.ScheduledTime);
                var recordToRun = triggers.Where(z => z.ScheduledTime == min).First();
                var result=recordToRun.Act(time);
                if (result == TriggerKeep.Remove)
                    triggers.Remove(recordToRun);
            }
            if (Ticked != null)
                Ticked();
        }

    }
}
