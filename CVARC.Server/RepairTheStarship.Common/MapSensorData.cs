using System.Runtime.Serialization;
using CVARC.Basic.Sensors;

namespace RepairTheStarship.Sensors
{
    [DataContract]
    public class MapSensorData : ISensorData
    {
        [DataMember]
        public MapItem[] MapItems { get; set; }
    }
}