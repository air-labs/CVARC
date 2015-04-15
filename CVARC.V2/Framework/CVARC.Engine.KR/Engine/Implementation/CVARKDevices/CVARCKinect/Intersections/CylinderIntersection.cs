using System;
using AIRLab.Mathematics;
using CVARC.Core;
using NUnit.Framework;

namespace kinect
{
    public partial class Intersector
    {
        public static double IntersectCylinder(Cylinder cylinder, Ray ray)
        {
            //Переменные для возврата корней квадратного уравнения
            double root1, root2;
            var a = ray.Direction.X * ray.Direction.X + ray.Direction.Y * ray.Direction.Y;
            var center = cylinder.GetAbsoluteLocation().ToPoint3D();
            var top = cylinder.GetAbsoluteLocation().ToPoint3D() + new Point3D(0, 0, cylinder.Height);
            //Вспомогательный вектор
            var vec = ray.Origin - center;
            var b = 2 * (ray.Direction.X * vec.X + ray.Direction.Y * vec.Y);
            var c = Math.Pow(ray.Origin.X - center.X, 2) +
                    Math.Pow(ray.Origin.Y - center.Y, 2) -
                    Math.Pow(cylinder.RBottom, 2);
            //Обработка решения
            if ((QuadEquation.Solution(a, b, c, out root1, out root2)) && ((root1 >= 0) | (root2 >= 0)))
                if (root1 >= 0)
                {
                    Point3D point = ray.CalculatePoint(root1); //если координата z лежит на цилиндре, то это нам подходит
                    if ((point.Z >= center.Z) && (point.Z <= center.Z + cylinder.Height))
                    {
                        return ray.DistanceTo(root1);
                    }
                    //если координата z за пределами, то проверяем пересечения с верхней и нижней гранью
                    else if (IntersectCircle(center, cylinder.RBottom, ray) != null)
                    {
                        return ray.Origin.Hypot(IntersectCircle(center, cylinder.RBottom, ray).Value);
                    }
                    else if (IntersectCircle(top, cylinder.RBottom, ray) != null)
                    {
                        return ray.Origin.Hypot(IntersectCircle(top, cylinder.RBottom, ray).Value);
                    }
                    else return double.PositiveInfinity;
                }
                else //все то же самое для другого корня
                {
                    Point3D point = ray.CalculatePoint(root2);
                    if ((point.Z >= center.Z) && (point.Z <= center.Z + cylinder.Height))
                    {
                        return ray.DistanceTo(root2);
                    }
                    else if (IntersectCircle(center, cylinder.RBottom, ray) != null)
                    {
                        return ray.Origin.Hypot(IntersectCircle(center, cylinder.RBottom, ray).Value);
                    }
                    else if (IntersectCircle(top, cylinder.RBottom, ray) != null)
                    {
                        return ray.Origin.Hypot(IntersectCircle(top, cylinder.RBottom, ray).Value);
                    }
                    else return double.PositiveInfinity;
                }
            else
            {
                return double.PositiveInfinity;
            }
        }
    }
    [TestFixture]
    internal class CylinderIntersectionTest
    {
        private static readonly Frame3D Location = new Frame3D(0, 0, 0);
        private static readonly Cylinder Cylinder = new Cylinder { Location = Location, RTop = 1, RBottom = 1, Height = 3 };

        private static readonly Frame3D Location1 = new Frame3D(2, 3, 1);
        private static readonly Cylinder Cylinder1 = new Cylinder { Location = Location1, RTop = 3, RBottom = 3, Height = 2 };


        private readonly object[][] _data4 = new []
            {
                new object[]
                    {
                        new Ray(new Point3D(0, 2, 1.5), new Point3D(0, -1, 0)), //пересечение
                        Cylinder,
                        1
                    },
                new object[]
                    {
                        new Ray(new Point3D(0, 2, 3), new Point3D(0, -1, 0)), //касание по грани
                        Cylinder,
                        1
                    },
                new object[]
                    {
                        new Ray(new Point3D(0, 2, 3 + 1e16), new Point3D(0, -1, 0)), //над гранью
                        Cylinder,
                        double.PositiveInfinity
                    },
                new object[]
                    {
                        new Ray(new Point3D(0, 2, 4), new Point3D(0, -1, 0)), //над гранью
                        Cylinder,
                        double.PositiveInfinity
                    },
                    new object[]
                    {
                        new Ray(new Point3D(1, 2, 1.5), new Point3D(0, -1, 0)), //касание по поверхности
                        Cylinder,
                        2
                    },
                    new object[]
                    {
                        new Ray(new Point3D(0, 2, 4), new Point3D(0, -1, -0.5)), //пересечесние с верхней крышкой
                        Cylinder,
                        Math.Sqrt(5)
                    },
                    new object[]
                    {
                        new Ray(new Point3D(5, 6, 2), new Point3D(-2, -2, 0)), //пересечесние c боковой поверхностью
                        Cylinder1,
                        1.24264068711929
                    },
                    new object[]
                    {
                        new Ray(new Point3D(5, 6, 2), new Point3D(-2, -2, 0.5)), //пересечесние c боковой поверхностью
                        Cylinder1,
                        1.26190758316504
                    },
                    new object[]
                    {
                        new Ray(new Point3D(5, 6, 4), new Point3D(-2, -2, -0.5)), //пересечесние c боковой поверхностью
                        Cylinder1,
                        Math.Sqrt(33)
                    }
            };


        [Test]
        [TestCaseSource("_data4")]
        public void RayTest(Ray ray, Cylinder cylinder, double distance)
        {
            var computedDistance = Intersector.IntersectCylinder(cylinder, ray);
            Assert.AreEqual(distance, computedDistance);
           // Console.WriteLine("Expected: {0}. But was: {1}", distance, computedDistance);
        }
    }
}
