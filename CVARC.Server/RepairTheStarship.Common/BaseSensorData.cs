using System.Runtime.Serialization;
using CVARC.Basic;
using CVARC.Basic.Sensors;
using CVARC.Basic.Sensors.Positions;

namespace RepairTheStarship.Sensors
{
    [DataContract]
    public class BaseSensorData : ISensorsData
    {
        [DataMember]
        public RobotIdSensorData RobotId { get; set; }

        [DataMember]
        public ManyPositionData Position { get; set; }

        [DataMember]
        public GripData DetailsInfo { get; set; }
    }
}