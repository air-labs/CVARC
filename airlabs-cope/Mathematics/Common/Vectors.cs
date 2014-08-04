using System;

namespace AIRLab.Mathematics {
    static partial class Geometry {
        public static double Hypot(double x, double y) {
            return Math.Sqrt(x * x + y * y);
        }

        public static double Hypot(double x, double y, double z) {
            return Math.Sqrt(x * x + y * y + z * z);
        }
        public static double Hypot(Point3D point)
        {
            return Hypot(point.X, point.Y, point.Z);
        }
        public static double Hypot(Frame3D frame)
        {
            return Hypot(frame.X, frame.Y, frame.Z);
        }

        public static double Hypot(Point2D frame)
        {
            return Hypot(frame.X, frame.Y);
        }

		public static double Hypot(Frame2D frame)
		{
			return Hypot(frame.X, frame.Y);
		}

        public static double Hypot(this Frame2D current, Frame2D another)
        {
            return Hypot(current.X - another.X, current.Y - another.Y);
        }

        public static double Hypot(this Point2D current, Point2D another) {
            return Hypot(current.X - another.X, current.Y - another.Y);
        }

        public static double Hypot(this Point3D current, Point3D another) {
            return Hypot(current.X - another.X, current.Y - another.Y, current.Z - another.Z);
        }


        public static Point3D Projection(Point3D toProject, Point3D axis) {
            if(axis.IsEmpty) throw new ArgumentException("Axis of projection cannot be null-vector");
            return (toProject.Norm() * Cos(AngleBetweenVectors(toProject, axis))) * axis.Normalize();
        }

        public static Point3D Projection(Point3D toProject, Plane plane) {
            return toProject - Projection(toProject, plane.Normal);
        }

        public static Point2D Projection(Point2D toProject, Point2D axis) {
            if(axis.IsEmpty) throw new ArgumentException("Axis of projection cannot be null-vector");
            return (toProject.Norm() * Cos(AngleBetweenVectors(toProject, axis))) * axis.Normalize();
        }


        public static Point3D Orthonorm(Point3D vector, Point3D axis) {
            return (vector - Projection(vector, axis)).Normalize();
        }
        public static Point2D Orthonorm(Point2D vector, Point2D axis) {
            return (vector - Projection(vector, axis)).Normalize();
        }


        public static Angle AngleBetweenVectors(Point3D firstVector, Point3D secondVector) {
            if(firstVector.IsEmpty || secondVector.IsEmpty)
                throw new ArgumentException("Vectors cannot be null-vector when calculating angle");
            var cos = firstVector.MultiplyScalar(secondVector) / (firstVector.Norm() * secondVector.Norm());
            if(Math.Abs(cos) > 1)
                cos = Math.Sign(cos) * 1;
            return Acos(cos);
        }

        public static Angle AngleBetweenVectors(Point2D firstVector, Point2D secondVector) {
            return AngleBetweenVectors(firstVector.ToPoint3D(), secondVector.ToPoint3D());
        }

        public static bool AreCollinear(Point3D a, Point3D b) {
            return Math.Abs(a.MultiplyVector(b).Norm() - 0) < Epsilon;
        }

        public static bool AreCollinear(Point2D a, Point2D b) {
            return Math.Abs(a.MultiplyVector(b).Norm() - 0) < Epsilon;
        }

    }
}
