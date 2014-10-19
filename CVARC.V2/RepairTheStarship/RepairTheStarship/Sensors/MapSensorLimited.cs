using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab;
using AIRLab.Mathematics;
using CVARC.Basic.Sensors;
using CVARC.V2;

namespace RepairTheStarship
{
    public class MapSensorLimited : MapSensor
    {
        double Distance(MapItem item)
        {
            var location=Actor.World.Engine.GetAbsoluteLocation(Actor.ObjectId);
            return Math.Sqrt(Math.Pow(item.X - location.X, 2) + Math.Pow(item.Y - location.Y, 2));
        }

        public override MapItem[] Measure()
        {
            var items = base.Measure();
            return items.Where(z => Distance(z) < 100).ToArray();
        }
    }
}
