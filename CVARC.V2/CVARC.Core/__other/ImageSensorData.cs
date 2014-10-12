using System.Runtime.Serialization;

namespace CVARC.Basic.Sensors
{
    [DataContract]
    public class ImageSensorData : ISensorData
    {
        [DataMember]
        public byte[] Bytes { get; set; }
        
        public ImageSensorData(byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}