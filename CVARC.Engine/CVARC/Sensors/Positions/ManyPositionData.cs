using System.Runtime.Serialization;

namespace CVARC.Basic.Sensors
{
    [DataContract]
    public class ManyPositionData : ISensorData
    {
        [DataMember]
        public PositionData[] PositionsData { get; set; }

        public ManyPositionData(PositionData[] data)
        {
            PositionsData = data;
        }
    }
}