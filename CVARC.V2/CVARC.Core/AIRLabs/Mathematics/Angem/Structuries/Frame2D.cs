using System;
using AIRLab.Mathematics;

namespace AIRLab.Mathematics
{
    [Serializable]
    public struct Frame2D
    {
        public readonly Angle Angle;
        public readonly Point2D Center;
        public readonly double X;
        public readonly double Y;

        public Frame2D(double x, double y, Angle angle)
        {
            X = x;
            Y = y;
            Angle = angle;
            Center = new Point2D(x, y);
        }

        public Frame3D ToFrame3D()
        {
            return new Frame3D(X, Y, 0, new Angle(), Angle, new Angle());
        }

        public Point2D ToPoint2D()
        {
            return new Point2D(X, Y);
        }

        public Frame2D Revert()
        {
            return ToFrame3D().Revert().ToFrame2D();
        }

        public Frame2D Invert()
        {
            return new Frame2D(
                -X*Geometry.Cos(Angle) - Y*Geometry.Sin(Angle),
                X*Geometry.Sin(Angle) - Y*Geometry.Cos(Angle),
                -Angle);
        }

        public Frame2D Apply(Frame2D arg)
        {
            var cos = Geometry.Cos(Angle);
            var sin = Geometry.Sin(Angle);
            return new Frame2D(X + arg.X*cos - arg.Y*sin, Y + arg.X*sin + arg.Y*cos, Angle + arg.Angle);
        }

        public Point2D Apply(Point2D arg)
        {
            var res = Apply(arg.ToFrame2D());
            return new Point2D(res.X, res.Y);
        }

        public Line2D Apply(Line2D arg)
        {
            return new Line2D(Apply(arg.Begin), Apply(arg.End));
        }

        public Frame2D Diff(Frame2D previos)
        {
            var x = previos.X - X;
            var y = previos.Y - Y;
            var angle = Angle.FromGrad(previos.Angle.Radian - Angle.Radian);
            return new Frame2D(x,y,angle);
        }

        public override string ToString()
        {
            return String.Format("X = {0:0.00}, Y = {1:0.00}, Angle = {2:0.00}", X, Y, Angle.Grad);
        }

        public bool IsBetween(Frame2D previousLocation, Frame2D currentLocation)
        {
            var maxDist = previousLocation.Hypot(currentLocation);
            if (this.Hypot(previousLocation) <= maxDist && this.Hypot(previousLocation) < maxDist)
                return true;
            return false;
        }

        #region Arithmetical operations

        public static Frame2D operator +(Frame2D a, Frame2D b)
        {
            return new Frame2D(a.X + b.X, a.Y + b.Y, a.Angle + b.Angle);
        }

        public static Frame2D operator -(Frame2D a, Frame2D b)
        {
            return new Frame2D(a.X - b.X, a.Y - b.Y, a.Angle - b.Angle);
        }

        public static Frame2D operator *(Frame2D a, double l)
        {
            return new Frame2D(a.X*l, a.Y*l, a.Angle*l);
        }

        public static Frame2D operator *(double l, Frame2D a)
        {
            return a*l;
        }

        public static Frame2D operator /(Frame2D a, double l)
        {
            return a*(1/l);
        }

        #endregion

        #region Equality members

        public static bool operator ==(Frame2D a, Frame2D b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Frame2D a, Frame2D b)
        {
            return !(a == b);
        }

        public bool Equals(Frame2D other)
        {
            const double epsilon = 0.00001;
            //Если по модулю Frame2D находятся в окрестности epsilon то они равны
            return Math.Abs(X - other.X) < epsilon && Math.Abs(Y - other.Y) < epsilon &&
                   Math.Abs(Angle.Radian - other.Angle.Radian) < epsilon;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Frame2D)) return false;
            return Equals((Frame2D) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = X.GetHashCode();
                result = (result*397) ^ Y.GetHashCode();
                result = (result*397) ^ Angle.GetHashCode();
                return result;
            }
        }

        #endregion

        #region Change operations

        public Frame2D NewX(double newX)
        {
            return new Frame2D(newX, Y, Angle);
        }

        public Frame2D NewY(double newY)
        {
            return new Frame2D(X, newY, Angle);
        }

        public Frame2D NewA(Angle newA)
        {
            return new Frame2D(X, Y, newA);
        }

        public Frame2D NewPoint(Point2D newPoint)
        {
            return new Frame2D(newPoint.X, newPoint.Y, Angle);
        }

        public Frame2D NewPoint(double newX, double newY)
        {
            return new Frame2D(newX, newY, Angle);
        }

        #endregion
    }
}