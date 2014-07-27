using System.Runtime.Serialization;
using CVARC.Basic;
using CVARC.Basic.Sensors;
using System.Linq;
using AIRLab.Mathematics;

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
            return new MapSensorData(Engine);
        }
    }

    [DataContract]
    public class MapSensorData : ISensorData
    {
        public MapSensorData(IEngine engine)
        {
            MapItems = engine.GetAllObjects()
                           .Select(e => new MapItem(e,engine.GetAbsoluteLocation(e)))
                           .Where(x => x.Tag != null)
                           .ToArray();
        }

        [DataMember]
        public MapItem[] MapItems { get; set; }
    }

    [DataContract]
    public class MapItem
    {
        [DataMember]
        public string Tag { get; set; }

        [DataMember]
        public double X { get; set; }

        [DataMember]
        public double Y { get; set; }

        public MapItem(string id, Frame3D Location)
        {
            var name = id.Split('_').First();
            switch (name)
            {
                case "DR": Tag = "RedDetail"; break;
                case "DB": Tag = "BlueDetail"; break;
                case "DG": Tag = "GreenDetail"; break;
                case "VW": Tag = "VerticalWall"; break;
                case "VWR": Tag = "VerticalRedSocket"; break;
                case "VWB": Tag = "VerticalBlueSocket"; break;
                case "VWG": Tag = "VerticalGreenSocket"; break;
                case "HW": Tag = "HorizontalWall"; break;
                case "HWR": Tag = "HorizontalRedSocket"; break;
                case "HWB": Tag = "HorizontalBlueSocket"; break;
                case "HWG": Tag = "HorizontalGreenSocket"; break;
            }
            X = Location.X;
            Y = Location.Y;
        }

        public override string ToString()
        {
            return string.Format("Tag: {0} X:{1} Y:{2}", Tag, X, Y);
        }
    }
}
