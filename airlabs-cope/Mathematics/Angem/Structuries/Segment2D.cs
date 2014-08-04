using System;

namespace AIRLab.Mathematics
{
    [Serializable]
    public struct Segment2D
    {
        public Segment2D(Point2D start, Point2D finish) : this()

        {
            Finish = finish;
            Start = start;
        }

        public Point2D Start { get; private set; }
        public Point2D Finish { get; private set; }

        public double Length
        {
            get { return Start.GetDistanceTo(Finish); }
        }

        public override string ToString()
        {
            return String.Format("{0}->{1}", Start, Finish);
        }
    }
}