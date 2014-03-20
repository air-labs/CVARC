using System.Runtime.Serialization;
using CVARC.Basic;
using CVARC.Basic.Sensors;

namespace Gems.Sensors
{
    [DataContract]
    public class SensorsData : ISensorsData
    {
        [DataMember]
        public MapSensorData MapSensor { get; set; }

        [DataMember]
        public RobotIdSensorData RobotIdSensor { get; set; }

        [DataMember]
        public ManyPositionData LightHouseSensor { get; set; }
    }
}
