using System;
using System.Collections.Generic;
using System.Drawing;
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
        const double SensorLimit = 70;

        Frame3D robotLocation;

        PointF Turn(PointF point)
        {
            var frame = new Frame3D(point.X, point.Y, 0);
            frame = robotLocation.Invert().Apply(frame);
            return new PointF((float)frame.X, (float)frame.Y);
        }

        double Hypot(PointF point)
        {
            return Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Y, 2));
        }

        bool Visible(PointF point)
        {
            if (point.X < Math.Abs(point.Y)) return false;
            return Hypot(point) < SensorLimit;
        }

        

        DetailMapData Process(DetailMapData data)
        {
            data.Location = Turn(data.Location);
            if (!Visible(data.Location)) return null;
            return data;
        }

        WallMapData Process(WallMapData data)
        {
            data.Center = Turn(data.Center);
            data.FirstEnd = Turn(data.FirstEnd);
            data.SecondEnd = Turn(data.SecondEnd);
            if (Visible(data.Center) || Visible(data.FirstEnd) || Visible(data.SecondEnd)) return data;
            return null;
        }


        public override Map Measure()
        {
            robotLocation = Actor.World.Engine.GetAbsoluteLocation(Actor.ObjectId);
            var map=base.Measure();
            map.Details = map.Details
                .Select(Process)
                .Where(z=>z!=null)
                .ToArray();
            map.Walls = map.Walls
                .Select(Process)
                .Where(z => z != null)
                .ToArray();
            return map;
        }
    }
}
