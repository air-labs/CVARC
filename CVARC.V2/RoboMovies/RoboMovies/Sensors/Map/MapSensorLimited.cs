using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AIRLab;
using AIRLab.Mathematics;
using CVARC.Basic.Sensors;
using CVARC.V2;

namespace RoboMovies
{
    public class MapSensorLimited : MapSensor
    {
        const double SensorLimit = 70;

		double Hypot(PointF point)
		{
			return Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Y, 2));
		}

		bool Visible(PointF point)
		{
			if (point.X < Math.Abs(point.Y)) return false;
			return Hypot(point) < SensorLimit;
		}


        public override Map Measure()
        {
            var map=base.Measure();
            map.Details = map.Details
                .Where(z=>Visible(z.Location))
                .ToArray();
            map.Walls = map.Walls
                .Where(z => Visible(z.FirstEnd) || Visible(z.SecondEnd) || Visible(z.Center))
                .ToArray();
            return map;
        }
    }
}
