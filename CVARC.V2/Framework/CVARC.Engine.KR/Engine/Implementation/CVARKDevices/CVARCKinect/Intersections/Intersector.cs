using System;
using AIRLab.Mathematics;
using CVARC.Core;
using NUnit.Framework;
using eBall = CVARC.Core.Ball;
using eBox = CVARC.Core.Box;
using ePlane = AIRLab.Mathematics.Plane;
using eCylinder = CVARC.Core.Cylinder;
using eAngle = AIRLab.Mathematics.Angle;

namespace kinect
{
    public partial class Intersector
    {
       #region CircleMethod
        /// <summary>
        /// Возвращает точку пересечения луча и круга параллельного плоскости z = 0
        /// </summary>
        /// <param name="center">Вершина</param>
        /// <param name="radius">Радиус</param>
        /// <param name="ray">Луч</param>
        /// <returns></returns>
        public static Point3D? IntersectCircle(Point3D center, double radius, Ray ray)
        {
            var p1 = new Point3D(center.X - radius, center.Y, center.Z);
            var p2 = new Point3D(center.X, center.Y - radius, center.Z);
            var plane = new ePlane(center, p1, p2);
            
            Point3D? point1 = IntersectPlane(plane, ray);
            
            if ((point1 != null) &&
                (center.Hypot(point1.Value) <= radius))
            {
                return point1;
            }
            else
            {
                return null;
            }
        }
        #endregion CircleMethod  
       #region TriangleMethod
        internal static class TriangleIntersection
        {
            //Определение плоскости для проецирования треугольника
            private static int ProjectPlane(ePlane plane)
            {
                var n = plane.Normal; //Вектор нормали
                if ((Math.Abs(n.X) >= Math.Abs(n.Y))
                    && (Math.Abs(n.X) >= Math.Abs(n.Z))) return 0; //0 - наибольшая Nx - проецирование на yOz
                else if ((Math.Abs(n.Y) >= Math.Abs(n.Z))
                         && (Math.Abs(n.Y) >= Math.Abs(n.X))) return 1; //1 - наибольшая Ny - проецирование на xOz
                else return 2; //2 - наибольшая Nz - проецирование на xOy
            }

            //Проецирование точек на соответствующую плоскость
            private static Point2D  ProjectPoint(int i, Point3D p) //i - плоскость проектирования
            {
                switch (i)
                {
                    case 0:
                        return new Point2D(p.Y, p.Z); //0 - проецирование на yOz
                    case 1:
                        return new Point2D(p.X, p.Z); //1 - проецирование на xOz
                    case 2:
                        return new Point2D(p.X, p.Y); //2 - проецирование на xOy
                    default: throw new Exception("Invalid plane for projection");
                }
            }
            /// <summary>
            /// Возвращает точку пересечения луча и треугольника p0p1p2
            /// </summary>
            /// <param name="p0">Вершина</param>
            /// <param name="p1">Вершина</param>
            /// <param name="p2">Вершина</param>
            /// <param name="ray">Луч</param>
            /// <returns></returns>
            public static Point3D? IntersectTriangle(Point3D p0, Point3D p1, Point3D p2, Ray ray)
            {
                var plane = new ePlane(p0, p1, p2); //Плоскость содержащая треугольник
                Point3D? point1 = IntersectPlane(plane, ray);
                if (point1 == null)
                {
                    return null;
                }
                else
                {
                    int i = ProjectPlane(plane); //Определяем куда проецировать треугольник
                    Point2D point = ProjectPoint(i, point1.Value);
                    Point2D v0 = ProjectPoint(i, p0);
                    Point2D v1 = ProjectPoint(i, p1);
                    Point2D v2 = ProjectPoint(i, p2);
                    Point2D[] regionPoints = new Point2D[] {v0, v2, v1};
                    return Geometry.IsFromRegion(point, regionPoints) ? point1 : null;
                }
            }
        }
        #endregion TriangleMethod

        public static double Intersect(Body body, Ray ray)
        {
            if (body is Box)
                return IntersectBox(body as Box, ray);
            if (body is Ball)
                return IntersectBall(body as Ball, ray);
            if (body is Cylinder)
                return IntersectCylinder(body as Cylinder, ray);
            return double.PositiveInfinity;
            //throw new Exception("Unknown body");
        }
    }
    #region CircleTest
    [TestFixture]
    internal class CircleIntersectionTest
    {
        private static readonly Point3D Center1 = new Point3D(0, 0, 0);
        private static double radius1 = 2.0;
        private static readonly Point3D Center2 = new Point3D(2, 3, 1);
        private static double radius2 = 3.0;
        private readonly object[][] _data3 = new []
            {
                new object[]
                    {
                        new Ray(new Point3D(0, 3, 0),new Point3D(0, -1, 0)),
                        Center1,
                        radius1,
                        null
                    }, 
                new object[]
                    {
                        new Ray(new Point3D(0, 3, 1),new Point3D(0, -1, 0)),
                        Center1,
                        radius1,
                        null
                    },
                    new object[]
                    {
                        new Ray(new Point3D(0, 3, 1),new Point3D(0, -1, -1)),
                        Center1,
                        radius1,
                        new Point3D(0, 2, 0)
                    },
                    new object[]
                    {
                        new Ray(new Point3D(0, 3, 1),new Point3D(0, -1, -0.5)),
                        Center1,
                        radius1,
                        new Point3D(0, 1, 0)
                    },
                    new object[]
                    {
                        new Ray(new Point3D(3, 7, 2),new Point3D(-2, -1, -1)),
                        Center2,
                        radius2,
                        null
                    },
                    new object[]
                    {
                        new Ray(new Point3D(3, 7, 2),new Point3D(-2, -3, -1)),
                        Center2,
                        radius2,
                        new Point3D(1, 4, 1)
                    }
            };
        [Test]
        [TestCaseSource("_data3")]
        public void RayTest(Ray ray, Point3D center, double radius, Point3D? point)
        {
            Point3D? computedPoint = Intersector.IntersectCircle(center, radius, ray);
            Assert.AreEqual(point, computedPoint);
            {
                //Console.WriteLine("Point:  {0}. Expected: {1}", computedPoint, point);
            }
        }
    }
    #endregion CircleTest
    #region TriangleTest
    [TestFixture]
    internal class TriangleIntersectionTest
    {
        private static readonly Point3D p0 = new Point3D(1, 0, 0);
        private static readonly Point3D p2 = new Point3D(1, 1, 1);
        private static readonly Point3D p1 = new Point3D(1, 0, 1);

        private static readonly Point3D v0 = new Point3D(1, 0, 0);
        private static readonly Point3D v2 = new Point3D(0, 1, 0);
        private static readonly Point3D v1 = new Point3D(0, 0, 1);
        //наклон в другую сторону
        private static readonly Point3D t0 = new Point3D(1, 0, 0);
        private static readonly Point3D t2 = new Point3D(0, 1, 0);
        private static readonly Point3D t1 = new Point3D(1, 1, 1);

        private readonly object[][] _data6 = new []
            {
                new object[]
                    {
                        t0, t1, t2,
                        new Ray(new Point3D(0.5, 2, 0.1), new Point3D(-0.1, -1, 0.1)),//ничему не параллелен
                        new Point3D(2.3/6.0, 5.0/6.0, 1.3/6.0)
                    },
                new object[]
                    {
                        v0, v1, v2,
                        new Ray(new Point3D(2, 0.75, 0.1), new Point3D(-2, -0.25, 0.1)),//ничему не параллелен
                        new Point3D(12.0/43.0, 23.0/43.0, 8.0/43.0)
                    },
                new object[]
                    {
                        v0, v1, v2,
                        new Ray(new Point3D(0.5, 1.5, 0), new Point3D(0.5, -2, 0)),//пересекает плоскость
                        new Point3D(5.0/6.0, 1.0/6.0, 0)
                    },
                new object[]
                    {
                        v0, v1, v2,
                        new Ray(new Point3D(1, 3, 0), new Point3D(1, -1, 0)),//паралелен плоскости
                        null
                    },
                new object[]
                    {
                        v0, v1, v2,
                        new Ray(new Point3D(0, 0, 0), new Point3D(1, 1, 1)),//ничему не параллелен
                        new Point3D(1.0/3.0, 1.0/3.0, 1.0/3.0)
                    },
                new object[]
                    {
                        p0, p1, p2,
                        new Ray(new Point3D(0, 0, 0), new Point3D(1, 0, 0)),//направлен в вершину
                        new Point3D(1, 0, 0)
                    },
                new object[]
                    {
                        p0, p1, p2,
                        new Ray(new Point3D(0, 0, 0), new Point3D(1, 0, 0.5)),//направлен в сторону
                        new Point3D(1, 0, 0.5)
                    },
                new object[]
                    {
                        p0, p1, p2,
                        new Ray(new Point3D(0, 0, 0), new Point3D(1, 0.1, 0.5)),//направлен внутрь
                        new Point3D(1, 0.1, 0.5)
                    },
                new object[]
                    {
                        p0, p1, p2,
                        new Ray(new Point3D(0, 0, 0), new Point3D(0, 1, 0)),//направлен мимо
                        null
                    }
            };
        [Test]
        [TestCaseSource("_data6")]
        public void RayTest(Point3D v0, Point3D v1, Point3D v2, Ray ray, Point3D? point)
        {
            var computedPoint = Intersector.TriangleIntersection.IntersectTriangle(v0, v1, v2, ray);
            Assert.AreEqual(point, computedPoint);
          //  Console.WriteLine("Expected: {0}. But was: {1}", point, computedPoint);
        }
    }
    #endregion TriangleTest
}