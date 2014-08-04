using System;
using AIRLab.Mathematics;

namespace AIRLab.Mathematics
{
    [Serializable]
    public struct Point3D
    {
        public readonly bool IsEmpty;
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        #region Creation

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
            IsEmpty = Math.Abs(X - 0) < Geometry.Epsilon && Math.Abs(Y - 0) < Geometry.Epsilon && Math.Abs(Z - 0) < Geometry.Epsilon;
        }

        #endregion

        #region Conversion

        public Frame3D ToFrame()
        {
            return new Frame3D(X, Y, Z, new Angle(), new Angle(), new Angle());
        }

        #endregion

        #region Arithmetical operations

        public static Point3D operator +(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public static Point3D operator -(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static Point3D operator -(Point3D p)
        {
            return new Point3D(-p.X, -p.Y, -p.Z);
        }

        public static Point3D operator *(Point3D p, double l)
        {
            return new Point3D(p.X*l, p.Y*l, p.Z*l);
        }

        public static Point3D operator *(double l, Point3D p)
        {
            return p*l;
        }

        public static Point3D operator /(Point3D p, double l)
        {
            return p*(1/l);
        }
        
        public static bool operator ==(Point3D firstPoint, Point3D secondPoint)
        {
            return Math.Abs(firstPoint.X - secondPoint.X) < Geometry.Epsilon &&
                   Math.Abs(firstPoint.Y - secondPoint.Y) < Geometry.Epsilon &&
                   Math.Abs(firstPoint.Z - secondPoint.Z) < Geometry.Epsilon;
        }

        public static bool operator !=(Point3D firstPoint, Point3D secondPoint)
        {
            return !(firstPoint == secondPoint);
        }

        public double MultiplyScalar(Point3D p)
        {
            return X*p.X + Y*p.Y + Z*p.Z;
        }

        public Point3D MultiplyVector(Point3D p)
        {
            return new Point3D(Y*p.Z - Z*p.Y, -X*p.Z + Z*p.X, X*p.Y - Y*p.X);
        }

        public static double TripleProduct(Point3D a, Point3D b, Point3D c)
        {
            return a.X*(b.Y*c.Z - b.Z*c.Y) - a.Y*(b.X*c.Z - b.Z*c.X) + a.Z*(b.X*c.Y - b.Y*c.X);
        }

        #endregion

        #region Projections

        public Point3D ProjectionOnAxis(Point3D axis)
        {
            if (axis.Norm() < double.Epsilon) throw new Exception("Axis length shouldn't be 0");
            return axis*axis.MultiplyScalar(this)/Math.Pow(axis.Norm(), 2);
        }

        public Point3D ProjectionOnPlane(Plane plane)
        {
            return this - ProjectionOnAxis(plane.Normal);
        }

        #endregion

        public double Norm()
        {
            return Geometry.Hypot(X, Y, Z);
        }

        public Point3D Normalize()
        {
            return this/Norm();
        }

        public  bool Equals(Point3D point)
        {
            return (Math.Abs(X - point.X) + Math.Abs(Y - point.Y) + Math.Abs(Z - point.Z)) < 1e-6;
        }

        public override string ToString()
        {
            return String.Format("X = {0}, Y = {1}, Z = {2}", X, Y, Z);
            //return MathIO.Point3D.ExtendedFormat.Write(this);
        }
    }
}