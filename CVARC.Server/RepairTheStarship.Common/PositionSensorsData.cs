using System.Runtime.Serialization;

namespace RepairTheStarship.Sensors
{
    [DataContract]
    public class PositionSensorsData : BaseSensorData
    {
        [DataMember]
        public MapSensorData MapSensor { get; set; }
    }
}
