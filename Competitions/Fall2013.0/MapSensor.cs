using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;
using AIRLab.Thornado;
using CVARC.Basic;
using CVARC.Basic.Sensors;
using CVARC.Core;
using CVARC.Graphics;

namespace StarshipRepair
{
    public class MapSensor : ISensor
    {
        private Body _root;

        public void Init(Robot robot, World wrld, DrawerFactory factory)
        {
            _root = robot.Body.TreeRoot;
        }

        public ISensorData Measure()
        {
            return new MapSensorData(_root);
        }
    }

    public class MapSensorData : ISensorData
    {
        Body root;

        public MapSensorData(Body root)
        {
            this.root = root;
        }

        public string GetStringRepresentation()
        {
            var result = "<Map>";
            foreach (var e in root.GetSubtreeChildrenFirst())
            {
                string tag = null;
                switch (e.Name)
                {
                    case "DR": tag = "RedDetail"; break;
                    case "DB": tag = "BlueDetail"; break;
                    case "DG": tag = "GreenDetail"; break;
                    case "VW": tag = "VerticalWall"; break;
                    case "VWR": tag = "VerticalRedSocket"; break;
                    case "VWB": tag = "VerticalBlueSocket"; break;
                    case "VWG": tag = "VerticalGreenSocket"; break;
                    case "HW": tag = "HorizontalWall"; break;
                    case "HWR": tag = "HorizontalRedSocket"; break;
                    case "HWB": tag = "HorizontalBlueSocket"; break;
                    case "HWG": tag = "HorizontalGreenSocket"; break;
                }
                if (tag == null) continue;
                result += string.Format("<{0}><X>{1}</X><Y>{2}</Y></{0}>", tag, e.Location.X, e.Location.Y);
            }
            result += "</Map>";
            return result;
        }
    }
}
