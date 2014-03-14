using System.Runtime.Serialization;

namespace CVARC.Basic.Sensors
{
    [DataContract]
    public class ImageSensorData : ISensorData
    {
        [DataMember]
        public string Base64Picture { get; set; }
    }
}