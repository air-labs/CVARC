using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{

    public static class IWorldExtensions
    {
        public static void RunWithoutInterface(this IWorld world)
        {
            bool stop = false;
            world.Exit += () => stop = true;
            var oldTime = 0.0;
            while (!stop)
            {
                var time = world.Clocks.GetNextEventTime();
                if (time > world.Configuration.Settings.TimeLimit) break;
                (world.Engine as KroREngine).Updates(oldTime, time);
                world.Clocks.Tick(time);
                oldTime = time;
            }
        }
    }
}
