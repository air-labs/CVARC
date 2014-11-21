using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface IWorld
    {
        IEngine Engine { get; }
        IWorldManager Manager { get; }
        void Initialize(Competitions competitions, IRunMode environment);
        WorldClocks Clocks { get; }
        IdGenerator IdGenerator { get; }
        Scores Scores { get; }
        IEnumerable<IActor> Actors { get; }
        void OnExit();
        event Action Exit;
        IRunMode RunMode { get; }
        Logger Logger { get; }
    }

    public static class IWorldExtensions
    {
        public static void RunWithoutInterface(this IWorld world)
        {
            bool stop = false;
            world.Exit += () => stop = true;
            while (!stop)
            {
                var time = world.Clocks.GetNextEventTime();
                if (time > world.RunMode.Configuration.Settings.TimeLimit) break;
                world.Clocks.Tick(time);
            }
        }
    }
}
