using System.Runtime.Serialization;
using CVARC.Core;
using CVARC.Graphics;
using System.Linq;

namespace CVARC.Basic.Sensors
{
    public class MapSensor : ISensor<MapSensorData>
    {
        private Body root;

        public void Init(Robot robot, World wrld, DrawerFactory factory)
        {
            root = robot.Body.TreeRoot;
        }

        public MapSensorData Measure()
        {
            return new MapSensorData(root);
        }
    }

    [DataContract]
    public class MapSensorData : ISensorData
    {
        private readonly Body root;

        public MapSensorData(Body root)
        {
            this.root = root;
            MapItems = root.GetSubtreeChildrenFirst()
                           .Select(e => new MapItem(e))
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

        public MapItem(Body body)
        {
            switch (body.Name)
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
            X = body.Location.X;
            Y = body.Location.Y;
        }
    }
}
