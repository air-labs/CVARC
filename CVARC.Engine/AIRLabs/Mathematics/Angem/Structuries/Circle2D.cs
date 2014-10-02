using System;
using AIRLab.Mathematics;

namespace AIRLab.Mathematics
{
    [Serializable]
    public struct Circle2D
    {
        public Circle2D(Point2D center, double r) : this()
        {
            Center = center;
            R = r;
        }

        public double R { get; private set; }
        public Point2D Center { get; private set; }

        public override string ToString()
        {
            return String.Format("Center {0} with r:{1}", Center, R);
        }
    }

    public struct Sector
    {
        public Sector(Circle2D circle2D, Point2D start, Point2D finish) : this()
        {
            if (PointOnCircle(start, circle2D) && PointOnCircle(finish, circle2D))
            {
                Finish = finish;
                Start = start;
                Circle = circle2D;
                Angle = new Triangle2D(Circle.Center, Start, Finish).AngleA;
                if (double.IsNaN(Angle.Radian))
                    Angle = Angle.Zero;
                return;
            }
            throw new Exception("Не возможно создать сектор");
        }

        public Circle2D Circle { get; private set; }
        public Point2D Start { get; private set; }
        public Point2D Finish { get; private set; }

        public Angle Angle { get; private set; }

        // вероятно, поворот по меньшему углу => вставил simplify
        public bool IsClockwise()
        {
            var startAngle = Geometry.GetAbsAngle(Start, Circle.Center);
            var finalAngle = Geometry.GetAbsAngle(Finish, Circle.Center);
            //return (finalAngle - startAngle).Radian >= Math.PI;
            return (finalAngle - startAngle).Simplify180().Radian < 0;
        }

        private static bool PointOnCircle(Point2D point, Circle2D c)
        {
            const double epsilon = 0.000001;
            double actualyR = point.GetDistanceTo(c.Center);
            return (actualyR < c.R + epsilon && actualyR > c.R - epsilon);
        }

        public override string ToString()
        {
            return String.Format("{0} from {1} to {2}", Circle, Start, Finish);
        }
    }
}