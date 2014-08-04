using System;
using AIRLab.Mathematics;

namespace AIRLab.Mathematics
{
    public static partial class Geometry
    {
        public static double Sin(Angle angle)
        {
            return Math.Sin(angle.Radian);
        }

        public static double Cos(Angle angle)
        {
            return Math.Cos(angle.Radian);
        }

        public static double Tg(Angle angle)
        {
            return Math.Tan(angle.Radian);
        }

        public static Angle Asin(double value)
        {
            return Angle.FromRad(Math.Asin(value));
        }

        public static Angle Acos(double value)
        {
            return Angle.FromRad(Math.Acos(value));
        }

        public static Angle Atan(double value)
        {
            return Angle.FromRad(Math.Atan(value));
        }

        public static Angle Atan2(double y, double x)
        {
            return Angle.FromRad(Math.Atan2(y, x));
        }
    }
}