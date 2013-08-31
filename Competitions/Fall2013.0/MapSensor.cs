using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using AIRLab.Thornado;
using CVARC.Basic;
using CVARC.Basic.Sensors;
using CVARC.Core;
using CVARC.Graphics;

namespace Gems
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
        private readonly List<Box> _walls;
        private readonly List<Box> _parts;
        private readonly List<Box> _places;

        public MapSensorData(Body root)
        {
            _walls = root.GetSubtreeChildrenFirst().Where(a => a.Name == "wall").OfType<Box>().ToList();
            _parts = root.GetSubtreeChildrenFirst().Where(a => a.Name == "part").OfType<Box>().ToList();
            _places = root.GetSubtreeChildrenFirst().Where(a => a.Name == "place").OfType<Box>().ToList();
        }

        public string GetStringRepresentation()
        {
            var str = "<walls>\r\n";
            str += string.Join("", _walls.Select(a => string.Format("<data>{0}<width>{1}</width><depth>{2}</depth><height>{3}</height></data>", new XML().WriteToString(a.GetAbsoluteLocation()), a.XSize, a.YSize, a.ZSize)));
            str += "\r\n</walls><parts>\r\n";
            str += string.Join("", _parts.Select(a => string.Format("<data>{0}<width>{1}</width><depth>{2}</depth><height>{3}</height></data>", new XML().WriteToString(a.GetAbsoluteLocation()), a.XSize, a.YSize, a.ZSize)));
            str += "\r\n</parts><places>\r\n";
            str += string.Join("", _places.Select(a => string.Format("<data>{0}<width>{1}</width><depth>{2}</depth><height>{3}</height><free>{4}</free></data>", new XML().WriteToString(a.GetAbsoluteLocation()), a.XSize, a.YSize, a.ZSize, a.Any() ? 0 : 1)));
            str += "\r\n</places>";
            return str;
        }
    }
}
