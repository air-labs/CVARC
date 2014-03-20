namespace CVARC.Basic.Sensors.Core
{
    public interface ISensor<out T> where T : ISensorData
    {
        T Measure();
    }
}
