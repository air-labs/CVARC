using System;

namespace AIRLab.Mathematics
{
    [Serializable]
    public struct Line3D
    {
        public readonly Point3D Begin;
        public readonly Point3D Direction;
        public readonly Point3D End;
        public readonly bool IsEmpty;

        public Line3D(Point3D begin, Point3D end)
        {
            Begin = begin;
            End = end;
            Direction = end - begin;
            IsEmpty = Direction.IsEmpty;
        }

        public static Line3D FromDirection(Point3D begin, Point3D direction)
        {
            return new Line3D(begin, begin + direction);
        }

        public Point3D GetCenter()
        {
            return (Begin + End)/2;
        }
    }
}