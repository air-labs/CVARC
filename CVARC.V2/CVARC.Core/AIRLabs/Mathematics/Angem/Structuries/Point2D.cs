#region

using System;
using System.Drawing;
using AIRLab.Mathematics;
//using NUnit.Framework;

#endregion

namespace AIRLab.Mathematics
{
    [Serializable]
    public struct Point2D
    {
        public readonly bool IsEmpty;
        public readonly double X;
        public readonly double Y;

        #region Creation

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
            IsEmpty = Math.Abs(X - 0) < Geometry.Epsilon && Math.Abs(Y - 0) < Geometry.Epsilon;
        }

        public Point2D(Point p)
            : this(p.X, p.Y)
        {
        }

        public Point2D(PointF p)
            : this(p.X, p.Y)
        {
        }

        #endregion

        #region Conversion

        public Point ToPoint()
        {
            return new Point((int)X, (int)Y);
        }

        public PointF ToPointF()
        {
            return new PointF((float)X, (float)Y);
        }

        public Point3D ToPoint3D()
        {
            return new Point3D(X, Y, 0);
        }

        public Frame2D ToFrame2D()
        {
            return new Frame2D(X, Y, new Angle());
        }

        #endregion

        #region Arithmetical operations

        public static Point2D operator +(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point2D operator -(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Point2D operator -(Point2D p)
        {
            return new Point2D(-p.X, -p.Y);
        }

        public static Point2D operator *(Point2D p, double l)
        {
            return new Point2D(p.X * l, p.Y * l);
        }

        public static Point2D operator *(double l, Point2D p)
        {
            return p * l;
        }

        public static Point2D operator /(Point2D p, double l)
        {
            return p * (1 / l);
        }

        public double MultiplyScalar(Point2D p)
        {
            return X * p.X + Y * p.Y;
        }

        public Point3D MultiplyVector(Point2D p)
        {
            return new Point3D(0, 0, X * p.Y - Y * p.X);
        }

        #endregion

        #region Equals and GetHashCode implementation

        public static bool operator ==(Point2D a, Point2D b)
        {
            const double epsilon = 0.000001;
            return GetDistance(a, b) < epsilon;
        }

        public static bool operator !=(Point2D a, Point2D b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return (obj is Point2D) && Equals((Point2D)obj);
        }

        public bool Equals(Point2D other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                hashCode += 1000000007 * X.GetHashCode();
                hashCode += 1000000009 * Y.GetHashCode();
                hashCode += 1000000021 * IsEmpty.GetHashCode();
            }
            return hashCode;
        }

        #endregion

        public double Norm()
        {
            return Geometry.Hypot(X, Y);
        }

        public Point2D Normalize()
        {
            return this / Norm();
        }

        public static double GetDistance(Point2D a, Point2D b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        public double GetLenghtVector()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public double GetDistanceTo(Point2D otherPoint)
        {
            return GetDistance(this, otherPoint);
            //return Math.Sqrt((X-otherPoint.X)*(X-otherPoint.X)+(Y-otherPoint.Y)*(Y-otherPoint.Y));
        }

        public PolarPoint2D ToPolarPoint2D()
        {
            double r = Math.Sqrt(X * X + Y * Y);
            Angle angle = Geometry.Atan2(Y, X);
            return new PolarPoint2D(r, angle);
        }

        public override string ToString()
        {
            return String.Format("(X={0:0.000};Y{1:0.000})", X, Y);
        }
    }

    //[TestFixture]
    //public class TestPoint2D
    //{
    //    private const double R = 10;
    //    private const double Epsilon = 0.0001;

    //    [Test]
    //    public void ГенерируемыйТестПроверящийPoint2DToPolarPoint2D()
    //    {
    //        for (double i = 0; i < 360; i = i + 0.1)
    //        {
    //            var expected = new PolarPoint2D(R, Angle.FromGrad(i));
    //            PolarPoint2D actual = expected.ToPoint2D().ToPolarPoint2D();
    //            Assert.That(actual.Alpha.Simplify360().Radian, Is.EqualTo(expected.Alpha.Radian).Within(Epsilon));
    //            Assert.That(actual.R, Is.EqualTo(expected.R).Within(Epsilon));
    //        }
    //    }
    //}
}