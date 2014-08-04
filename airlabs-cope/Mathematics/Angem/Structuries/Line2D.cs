using System;
using AIRLab.Mathematics;

namespace AIRLab.Mathematics
{
    [Serializable]
    public struct Line2D
    {
        public readonly Point2D Begin;
        public readonly Point2D Direction;
        public readonly Point2D End;
        public readonly double EqA;
        public readonly double EqB;
        public readonly double EqC;
        public readonly bool IsEmpty;
        public readonly Point2D Normal;

        public Line2D(Point2D begin, Point2D end)
        {
            Begin = begin;
            End = end;
            Direction = end - begin;
            Normal = new Point2D(-Direction.Y, Direction.X);
            EqA = Normal.X;
            EqB = Normal.Y;
            EqC = -EqA*begin.X - EqB*begin.Y;
            IsEmpty = Direction.IsEmpty;
        }

        private Line2D(double a, double b, double c, Point2D direction, Point2D start)
        {
            Begin = start;
            End = new Point2D();
            Direction = direction;
            Normal = new Point2D(-direction.Y, direction.X);
            EqA = a;
            EqB = b;
            EqC = c;
            IsEmpty = false;
        }

        public Line2D GetNormalLine(Point2D point)
        {
            double c = EqB*point.X - EqA*point.Y;
            var tempLine = new Line2D(-EqB, EqA, c, Normal, point);
            Point2D crossPoint = Geometry.GetCrossing(this, tempLine);
            return new Line2D(crossPoint, point);
        }

        public static Line2D FromDirection(Point2D begin, Point2D direction)
        {
            return new Line2D(begin, begin + direction);
        }

        public double GetXbyY(double y)
        {
            if (Math.Abs(Direction.Y - 0) < Geometry.Epsilon)
                throw new Exception("There are too many points or zero on line.");
            return Begin.X + Direction.X*(y - Begin.Y)/Direction.Y;
        }

        public double GetYbyX(double x)
        {
            if (Math.Abs(Direction.X - 0) < Geometry.Epsilon)
                throw new Exception("There are too many points or zero on line.");
            return Begin.Y + Direction.Y*(x - Begin.X)/Direction.X;
        }
    }
}