using CVARC.Basic.Sensors.Core;
using CVARC.Graphics;

namespace CVARC.Basic.Sensors
{
    public abstract class Sensor<T> : ISensor<T> where T : ISensorData
    {
        protected World World;
        protected readonly Robot Robot;
        protected readonly IEngine Engine;

        protected Sensor(Robot robot, World world)
        {
            World = world;
            Robot = robot;
            Engine = World.Engine;
        }

        public abstract T Measure();
    }
}