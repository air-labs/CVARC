using CVARC.Graphics;

namespace CVARC.Basic.Sensors
{
    public interface ISensorFactory
    {
        Sensor GetOne(Robot robot, World wrld, DrawerFactory factory);
    }
}
