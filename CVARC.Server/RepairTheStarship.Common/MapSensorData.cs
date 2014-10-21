using System.Linq;
using System.Runtime.Serialization;
using CVARC.Basic;
using CVARC.Basic.Sensors;

namespace RepairTheStarship.Sensors
{
    [DataContract]
    public class MapSensorData : ISensorData
    {
        public MapSensorData(IEngine engine)
        {
            MapItems = engine.GetAllObjects()
                .Select(e => new MapItem(e.Type, engine.GetAbsoluteLocation(e.Id)))
                .Where(x => x.Tag != null)
                .ToArray();
        }

        [DataMember]
        public MapItem[] MapItems { get; set; }
    }
}