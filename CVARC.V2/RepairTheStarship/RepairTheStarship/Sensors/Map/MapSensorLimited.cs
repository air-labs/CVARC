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
        const double SensorLimit = 100;

        Frame3D robotLocation;

        PointF Turn(PointF point)
        {
            var frame = new Frame3D(point.X, point.Y, 0);
            frame = robotLocation.Invert().Apply(frame);
            return new PointF((float)frame.X, (float)frame.Y);
        }

        double Distance(PointF point)
        {
            return Math.Sqrt(Math.Pow(point.X - robotLocation.X, 2) + Math.Pow(point.Y - robotLocation.Y, 2));
        }

        public override Map Measure()
        {
            robotLocation = Actor.World.Engine.GetAbsoluteLocation(Actor.ObjectId);
            var map=base.Measure();
            map.Details = map.Details
                .Where(z => Distance(z.Location) < 100)
                .Select(z => {
                    z.Location = Turn(z.Location);
                    return z;
                }).ToArray();
            map.Walls = map.Walls
                .Where(z => Math.Min(Distance(z.Center), Math.Min(Distance(z.FirstEnd), Distance(z.SecondEnd))) < SensorLimit)
                .Select(z =>
                {
                    z.Center = Turn(z.Center);
                    z.FirstEnd = Turn(z.FirstEnd);
                    z.SecondEnd = Turn(z.SecondEnd);
                    return z;
                })
                .ToArray();
            return map;
        }
    }
}
