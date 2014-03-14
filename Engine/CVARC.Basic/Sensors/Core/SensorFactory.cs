using CVARC.Graphics;

namespace CVARC.Basic.Sensors
{
    public class SensorFactory<TSensor> : ISensorFactory
        where TSensor: ISensor<ISensorData>
    {
        public Sensor GetOne(Robot robot, World wrld, DrawerFactory factory)
        {
            return new Sensor(typeof (TSensor), robot, wrld, factory){Name = typeof(TSensor).Name};
        }
    }
}