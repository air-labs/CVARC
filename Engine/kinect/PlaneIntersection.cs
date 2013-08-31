using System;
using AIRLab.Mathematics;
using NUnit.Framework;

namespace kinect
{
    public partial class Intersector
    {
        public static Point3D? IntersectPlane(Plane plane, Ray ray)
        {
            var a = plane.Normal.X;
            var b = plane.Normal.Y;
            var c = plane.Normal.Z;
            var d = -a*plane.Center.X - b*plane.Center.Y - c*plane.Center.Z;
            //Console.WriteLine("{0} x , {1} y , {2} z , {3} ", a,b,c,d);
            if (Math.Abs(ray.Direction.MultiplyScalar(plane.Normal)) < Double.Epsilon)
                //Если N || лучу, то нет пересечения
            {
                return null;
            }
            else
            {
                double t = -(d + ray.Origin.MultiplyScalar(plane.Normal))/(ray.Direction.MultiplyScalar(plane.Normal));
                if (t < 0) //Проверка на то, что точка пересечения лежит с нужной стороны от начала луча
                {
                    return null;
                }
                else
                {
                    return ray.CalculatePoint(t);
                }
            }

        }
    }

    [TestFixture]
    internal class PlaneIntersectionTest
    {
        //плоскость z = 1
        private static readonly Point3D zV1 = new Point3D(2, 3, 1);
        private static readonly Point3D zV2 = new Point3D(2, 0, 1);
        private static readonly Point3D zV3 = new Point3D(-1, 3, 1);
        private static readonly Plane zPlane = new Plane(zV1, zV2, zV3);
        //плоскость x + y + z - 1 = 0
        private static readonly Point3D V1 = new Point3D(1, 0, 0);
        private static readonly Point3D V2 = new Point3D(0, 1, 0);
        private static readonly Point3D V3 = new Point3D(0, 0, 1);
        private static readonly Plane Plane = new Plane(V1, V2, V3);
        //плоскость -x -y + z + d = 0
        private static readonly Point3D v1 = new Point3D(1, 2, 3);
        private static readonly Point3D v2 = new Point3D(0, 1, 0);
        private static readonly Point3D v3 = new Point3D(1, 1, 1);
        private static readonly Plane Plane2 = new Plane(v1, v2, v3);
        //плоскость y - 1 = 0
        private static readonly Point3D v_1 = new Point3D(0, 1, 3);
        private static readonly Point3D v_2 = new Point3D(0, 1, 0);
        private static readonly Point3D v_3 = new Point3D(1, 1, 0);
        private static readonly Plane plane = new Plane(v_1, v_2, v_3);

        private readonly object[][] _data1 = new[]
            {
                new object[]
                    {
                        new Ray(new Point3D(3, 7, 2), new Point3D(-2, -3, -1)), //тест z = 1
                        zPlane,
                        new Point3D(1, 4, 1)
                    },
                new object[]
                    {
                        new Ray(new Point3D(0, 1, 0), new Point3D(0, -1, 0)), //луч с началом на плоскости
                        plane,
                        new Point3D(0, 1, 0)
                    },
                new object[]
                    {
                        new Ray(new Point3D(0, 3, 0), new Point3D(0, -1, 0)), //пересечение
                        plane,
                        new Point3D(0, 1, 0)
                    },
                new object[]
                    {
                        new Ray(new Point3D(0, 3, 0), new Point3D(1, 0, 1)), //параллелен плоскости
                        plane,
                        null
                    },
                new object[]
                    {
                        new Ray(new Point3D(0, 3, 0), new Point3D(0, 1, 0)), //направлен в другую сторону
                        plane,
                        null
                    },
                new object[]
                    {
                        //Луч параллелен OY
                        new Ray(new Point3D(0, 2, 1), new Point3D(0, -1, 0)),
                        Plane,
                        new Point3D(0, 0, 1)
                    },
                new object[]
                    {
                        //Луч параллелен OX
                        new Ray(new Point3D(1, 0.5, 0), new Point3D(-1, 0, 0)),
                        Plane,
                        new Point3D(0.5, 0.5, 0)
                    },
                new object[]
                    {
                        //Луч ничему не параллелен
                        new Ray(new Point3D(0, 0, 0), new Point3D(1, 1, 1)),
                        Plane,
                        new Point3D(1.0/3.0, 1.0/3.0, 1.0/3.0)
                    },
                new object[]
                    {
                        //Луч параллелен плоскости
                        new Ray(new Point3D(0, 0, 0), new Point3D(1, -1, 0)),
                        Plane,
                        null
                    },
                new object[]
                    {
                        //xz
                        new Ray(new Point3D(0, 0, 0), new Point3D(1, -1, 0)),
                        Plane2,
                        null
                    }
            };


        [Test]
        [TestCaseSource("_data1")]
        public void RayTest(Ray ray, Plane plane, Point3D? point)
        {
            Point3D? computedPoint = Intersector.IntersectPlane(plane, ray);
            Assert.AreEqual(point, computedPoint);
            {
                Console.WriteLine("Point:  {0}. Expected: {1}", computedPoint, point);
            }
        }
    }
}