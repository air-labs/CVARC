using System.Runtime.Serialization;
using CVARC.Basic.Sensors;

namespace RepairTheStarship.Sensors
{
    [DataContract]
    public class ImageSensorsData : BaseSensorData
    {
        [DataMember]
        public ImageSensorData Image { get; set; }
    }
}