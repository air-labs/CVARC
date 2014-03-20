using CVARC.Basic.Sensors.Core;
using CVARC.Graphics;

namespace CVARC.Basic.Sensors
{
    public abstract class Sensor<T> : ISensor<T> where T : ISensorData
    {
        protected World World;
        protected readonly Robot Robot;
        protected readonly DrawerFactory Factory;

        protected Sensor(Robot robot, World world, DrawerFactory factory)
        {
            World = world;
            Robot = robot;
            Factory = factory;
        }

        public abstract T Measure();
    }
}