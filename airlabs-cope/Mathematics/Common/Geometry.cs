using System;
using AIRLab.Mathematics;

namespace AIRLab.Mathematics
{
    public static partial class Geometry
    {
        public static readonly double Epsilon = 1e-10;

        public static Point2D GetCrossing(Line2D line1, Line2D line2)
        {
            double a1 = line1.Direction.X,
                   b1 = line1.Direction.Y,
                   a2 = line2.Direction.X,
                   b2 = line2.Direction.Y,
                   c1 = line2.Begin.X - line1.Begin.X,
                   c2 = line2.Begin.Y - line1.Begin.Y;

            double t1 = (b2*c1 - a2*c2)/(a1*b2 - a2*b1);

            return new Point2D(line1.Begin.X + a1*t1, line1.Begin.Y + b1*t1);
        }

        public static bool GetCrossing(Segment2D segment1, Segment2D segment2, out Point2D crossingPoint)
        {
            var line1 = new Line2D(segment1.Start, segment1.Finish);
            var line2 = new Line2D(segment2.Start, segment2.Finish);
            crossingPoint = GetCrossing(line1, line2);
            var p = (crossingPoint.X - segment1.Finish.X) / (segment1.Start.X - segment1.Finish.X);
            return (0 <= p && p <= 1) && (crossingPoint.Y.Equals((p * segment1.Start.Y - (1 - p) * segment1.Finish.Y)));
        }

        /*
         * There are some not obvious calculations.
         * With them we can find the closest points of 
         * two straits. Function returns mean point of 
         * them.
         * p11, p21 - some points of straits.
         * p12, p22 - some other points of straits.
         * a1, a2 - vectors of straits.
         */

        public static Line3D GetPerpendicular(Line3D line1, Line3D line2)
        {
            Point3D p11 = line1.Begin;
            Point3D p21 = line2.Begin;
            Point3D a1 = line1.Direction.Normalize();
            Point3D a2 = line2.Direction.Normalize();

            if (Math.Abs(a1.X - a2.X) < Epsilon && Math.Abs(a1.Y - a2.Y) < Epsilon &&
                Math.Abs(a1.Z - a2.Z) < Epsilon)
                throw new Exception("straits are parallel");

            var p12 = new Point3D(p11.X + a1.X, p11.Y + a1.Y, p11.Z + a1.Z);
            var p22 = new Point3D(p21.X + a2.X, p21.Y + a2.Y, p21.Z + a2.Z);

            double p1 = (p12.X - p11.X)*(p12.X - p11.X) + (p12.Y - p11.Y)*(p12.Y - p11.Y) +
                        (p12.Z - p11.Z)*(p12.Z - p11.Z);
            double p2 = (p12.X - p11.X)*(p22.X - p21.X) + (p12.Y - p11.Y)*(p22.Y - p21.Y) +
                        (p12.Z - p11.Z)*(p22.Z - p21.Z);
            double q1 =
                -((p22.X - p21.X)*(p12.X - p11.X) + (p22.Y - p21.Y)*(p12.Y - p11.Y) + (p22.Z - p21.Z)*(p12.Z - p11.Z));
            double q2 =
                -((p22.X - p21.X)*(p22.X - p21.X) + (p22.Y - p21.Y)*(p22.Y - p21.Y) + (p22.Z - p21.Z)*(p22.Z - p21.Z));
            double r1 = (p21.X - p11.X)*(p12.X - p11.X) + (p21.Y - p11.Y)*(p12.Y - p11.Y) +
                        (p21.Z - p11.Z)*(p12.Z - p11.Z);
            double r2 = (p21.X - p11.X)*(p22.X - p21.X) + (p21.Y - p11.Y)*(p22.Y - p21.Y) +
                        (p21.Z - p11.Z)*(p22.Z - p21.Z);

            double m = (q2*r1 - q1*r2)/(p1*q2 - p2*q1);
            double n = (p1*r2 - p2*r1)/(p1*q2 - p2*q1);

            double x1 = p11.X + m*(p12.X - p11.X);
            double y1 = p11.Y + m*(p12.Y - p11.Y);
            double z1 = p11.Z + m*(p12.Z - p11.Z);

            double x2 = p21.X + n*(p22.X - p21.X);
            double y2 = p21.Y + n*(p22.Y - p21.Y);
            double z2 = p21.Z + n*(p22.Z - p21.Z);

            return new Line3D(new Point3D(x1, y1, z1), new Point3D(x2, y2, z2));
        }


        /// <summary>
        ///   this method translates from Spherical coordinates system to orthonormal.
        /// </summary>
        /// <returns> </returns>
        public static Point3D FromSphericToOrthonorm(Angle phi, Angle psi, Double r)
        {
            double x = Sin(psi)*Cos(phi);
            double y = Sin(psi)*Sin(phi);
            double z = Cos(psi);

            return new Point3D(x, y, z)*r;
        }

        public static Tuple<Angle, Angle, Double> FromOrthonormToSpheric(Point3D point)
        {
            double r = Math.Sqrt(point.X*point.X + point.Y*point.Y + point.Z*point.Z);
            Angle psi = Acos(point.Z/r);
            Angle phi = Atan2(point.Y, point.X);
            return new Tuple<Angle, Angle, double>(phi, psi, r);
        }

        public static bool IsParallel(Line3D line1, Line3D line2)
        {
            return AreCollinear(line1.Direction, line2.Direction);
        }

        public static bool IsParallel(Line2D line1, Line2D line2)
        {
            return AreCollinear(line1.Direction, line2.Direction);
        }

        public static double Distance(Point3D point1, Point3D point2)
        {
            return (point2 - point1).Norm();
        }

        public static double Distance(Point2D point1, Point2D point2)
        {
            return (point2 - point1).Norm();
        }

        public static double Distance(Point3D point, Line3D line)
        {
            Point3D vector = point - line.Begin;
            return vector.MultiplyVector(line.Direction).Norm()/line.Direction.Norm();
        }

        public static double Distance(Point2D point, Line2D line)
        {
            Point2D vector = point - line.Begin;
            return vector.MultiplyVector(line.Direction).Norm()/line.Direction.Norm();
        }

        /// <summary>
        ///   Проверка, лежит ли точка внутри области, заданной массивом точек
        /// </summary>
        // надо что-то решить с точками на границе. Сейчас undefined behavior.
        public static bool IsFromRegion(this Point2D thisPoint, Point2D[] regionPoints)
        {
            double sum = 0;
            Angle lastAngle = (regionPoints[regionPoints.Length - 1] - thisPoint).ToPolarPoint2D().Alpha;
            foreach (var point in regionPoints)
            {
                Angle currentAngle = (point - thisPoint).ToPolarPoint2D().Alpha;
                sum += lastAngle.GetDiffAngle(currentAngle).Radian;
                lastAngle = currentAngle;
            }
            return Math.Abs(sum) > Math.PI; // если внутри, sum=2pi; если снаружи, sum=0
        }

        public static bool IsThreePointInOneLine(Point2D a, Point2D b, Point2D c)
        {
            return IsParallel(new Line2D(a, b), new Line2D(b, c));
        }

        public static Angle GetAbsAngle(Point2D startVector, Point2D endVector)
        {
            return (endVector - startVector).ToPolarPoint2D().Alpha;
        }

        // функция находит кратчайшее угловое расстояние от currentAngle до finishAngle
        // конкурс на хорошее название
        //   пример:        public Angle GetNearestTo(this Angle from, Angle to)
        //   использование: currentAngle.GetNearestTo(finishAngle)
        //
        public static Angle GetDiffAngle(this Angle currentAngle, Angle finishAngle)
        {
            return (finishAngle - currentAngle).Simplify180();
        }
    }
}