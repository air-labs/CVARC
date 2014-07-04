using CVARC.Basic.Sensors.Core;


namespace CVARC.Basic.Sensors
{
    public abstract class Sensor<T> : ISensor<T> where T : ISensorData
    {
        protected Competitions World;
        protected readonly Robot Robot;
        protected readonly IEngine Engine;

        protected Sensor(Robot robot)
        {
            Robot = robot;
            World = robot.Competitions;
            Engine = World.Engine;
           
            
        }

        public abstract T Measure();
    }
}