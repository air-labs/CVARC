using System;
using AIRLab.Mathematics;

namespace AIRLab.Mathematics
{
    [Serializable]
    public class Plane
    {
        public readonly Point3D Basis1;
        public readonly Point3D Basis2;
        public readonly Point3D Center;

        public readonly Point3D Normal;
        public readonly Point3D V1;
        public readonly Point3D V2;
        public readonly Point3D V3;

        public Plane(Point3D v1, Point3D v2, Point3D v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
            Center = v1;
            Basis1 = (v2 - v1).Normalize();
            Basis2 = Geometry.Orthonorm(v3 - v1, Basis1);
            Normal = Basis1.MultiplyVector(Basis2).Normalize();
        }

        public static Plane FromDirection(Point3D center, Point3D direction1, Point3D direction2)
        {
            return new Plane(center, center + direction1, center + direction2);
        }

        public static Plane FromNormalVector(Point3D center, Point3D normalVector)
        {
            var some = new Point3D(1, 1, 1);
            if (Geometry.AreCollinear(some, normalVector))
                some = new Point3D(1, 2, 1);
            Point3D bas1 = Geometry.Orthonorm(some, normalVector);
            return new Plane(center, center + bas1, center + bas1.MultiplyVector(normalVector));
        }
    }
}