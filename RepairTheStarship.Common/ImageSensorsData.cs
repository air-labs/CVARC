using System.Runtime.Serialization;
using CVARC.Basic;
using CVARC.Basic.Sensors;

namespace RepairTheStarship.Sensors
{
    [DataContract]
    public class ImageSensorsData : ISensorsData
    {
        [DataMember]
        public RobotIdSensorData RobotId { get; set; }

        [DataMember]
        public ManyPositionData Position { get; set; }

        [DataMember]
        public ImageSensorData Image { get; set; }
    }
}