using System;

namespace kinect
{
    internal static class QuadEquation
    {
        public static bool Solution(double a, double b, double c, out double root1, out double root2)
        {
            double d = Math.Pow(b, 2) - (4*a*c);
            if (d < 0)
            {
                root1 = double.PositiveInfinity;
                root2 = double.PositiveInfinity;
                return false;
            }
            if (Math.Abs(d - 0) < double.Epsilon)
            {
                root1 = -b/(2*a);
                root2 = root1;
                return true;
            }
            root1 = (-b - Math.Sqrt(d))/(2*a);
            root2 = (-b + Math.Sqrt(d))/(2*a);
            return true;
        }
    }
}