using System.Runtime.Serialization;
using CVARC.Graphics;

namespace CVARC.Basic.Sensors
{
    [DataContract]
    public class RobotIdSensorData : ISensorData
    {
        [DataMember]
        public int Id { get; set; }
    }


    public class RobotIdSensor : ISensor<RobotIdSensorData>
    {
        int Id;
        public void Init(Robot robot, World wrld, DrawerFactory factory)
        {
            Id = robot.Number;
        }

        public RobotIdSensorData Measure()
        {
            return new RobotIdSensorData { Id = Id };
        }
    }
}
