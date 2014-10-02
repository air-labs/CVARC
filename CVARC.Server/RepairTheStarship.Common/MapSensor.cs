using System.Linq;
using CVARC.Basic;
using CVARC.Basic.Sensors;

namespace RepairTheStarship.Sensors
{
    public class MapSensor : Sensor<MapSensorData>
    {
        public MapSensor(Robot robot) 
            : base(robot)
        {
        }

        public override MapSensorData Measure()
        {
            return new MapSensorData
                {
                    MapItems = Engine.GetAllObjects().Select(e => new MapItem(e.Type, Engine.GetAbsoluteLocation(e.Id)))
                        .Where(x => x.Tag != null).ToArray()
                };
        }
    }
}
