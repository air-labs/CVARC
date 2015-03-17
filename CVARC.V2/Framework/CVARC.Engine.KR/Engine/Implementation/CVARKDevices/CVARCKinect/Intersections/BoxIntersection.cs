using System;
using System.Collections.Generic;
using System.Linq;
using AIRLab.Mathematics;
using CVARC.Core;
using NUnit.Framework;

namespace kinect
{
    public partial class Intersector
    {
        public static Point3D[] GetDirection(Box box)
        {
            Matrix m = box.Location.GetMatrix();
            var vectors = new Point3D[3];
            vectors[0] = new Point3D(m[0, 0], m[1, 0], m[2, 0]);
            vectors[1] = new Point3D(m[0, 1], m[1, 1], m[2, 1]);
            vectors[2] = new Point3D(m[0, 2], m[1, 2], m[2, 2]);
            //var up = vectors[2];
            //var left = -vectors[1];
            //var front = vectors[0];
            return vectors;
        }

        public static Ball GetExternalSphere(Box box)
        {
            var vectors = GetDirection(box);
            var up = vectors[2];
            Point3D center = box.GetAbsoluteLocation().ToPoint3D() + (0.5*up*box.ZSize);
            double radius = Math.Sqrt(Math.Pow(0.5*box.ZSize, 2.0) + Math.Pow(0.5*box.YSize, 2.0) + Math.Pow(0.5*box.XSize, 2.0));
            Point3D locaction = center - up.Normalize()*radius;
            return new Ball(){Location = locaction.ToFrame(), Radius = radius};
        }

        public static List<Point3D> GetVertex(Box box)
        {
            var location = box.GetAbsoluteLocation().ToPoint3D();
            var x = box.XSize;
            var y = box.YSize;
            var z = box.ZSize;
            Point3D[] vectors = GetDirection(box);
            var nUp = vectors[2];
            var nLeft = -vectors[1];
            var nFront = vectors[0];
            var vertex = new List<Point3D>
                {
                    location + 0.5*nLeft*y - 0.5*nFront*x,
                    location + 0.5*nLeft*y + 0.5*nFront*x,
                    location - 0.5*nLeft*y - 0.5*nFront*x,
                    location + nUp*z + 0.5*nLeft*y - 0.5*nFront*x,
                    location - 0.5*nLeft*y + 0.5*nFront*x,
                    location + nUp*z - 0.5*nLeft*y - 0.5*nFront*x,
                    location + nUp*z + 0.5*nLeft*y + 0.5*nFront*x,
                    location + nUp*z - 0.5*nLeft*y + 0.5*nFront*x
                };
            return vertex;
        }

        public static double IntersectBox(Box box, Ray ray)
        {
            Ball ball = GetExternalSphere(box);
            if (double.IsPositiveInfinity(IntersectBall(ball, ray)))
                return double.PositiveInfinity;
            else
            {
                var points = new List<Point3D>();
                var vertex = GetVertex(box);
                int[][] order = new int[12][];
                order[0] = new[] {0, 2, 1};
                order[1] = new[] {1, 2, 4};
                order[2] = new[] {0, 5, 2};
                order[3] = new[] {0, 3, 5};
                order[4] = new[] {0, 3, 1};
                order[5] = new[] {1, 3, 6};
                order[6] = new[] {3, 5, 6};
                order[7] = new[] {6, 5, 7};
                order[8] = new[] {1, 6, 7};
                order[9] = new[] {1, 7, 4};
                order[10] = new[] {2, 5, 7};
                order[11] = new[] {2, 7, 4};
                for (int i = 0; i < 12; i++)
                {
                    Point3D? point = TriangleIntersection.IntersectTriangle(vertex[order[i][0]], vertex[order[i][1]],
                                                                            vertex[order[i][2]], ray);
                    if (point != null)
                        points.Add(point.Value);
                    else 
                    {
                        point = TriangleIntersection.IntersectTriangle(vertex[order[i][0]], vertex[order[i][2]],
                                                                            vertex[order[i][1]], ray);
                        if (point != null)
                            points.Add(point.Value);
                    }
                }
                if (points.Capacity == 0)
                {
                    return double.PositiveInfinity;
                }
                return points.Min(a => ray.Origin.Hypot(a));
            }
        }
    }
    [TestFixture]
    internal class BoxIntersectionTest
    {
        private static readonly Frame3D Location = new Frame3D(0.5, 0.5, 0);
        private static readonly Box Box = new Box { Location = Location, XSize = 1, YSize = 1, ZSize = 1 };

        private static readonly Frame3D Location1 = new Frame3D(0.5, 0.5, 0.5, new Angle(), new Angle(), Angle.FromGrad(45));
        private static readonly Box Box1 = new Box
            {
                Location = Location1,
                XSize = Math.Sqrt(2),
                YSize = Math.Sqrt(2),
                ZSize = Math.Sqrt(2)
            };

        private readonly object[][] _data7 = new []
            {
                new object[]
                    {
                        new Ray(new Point3D(0.5, 3, 1), new Point3D(0, -1, 0)),
                        Box1,
                        2
                    },
                new object[]
                    {
                        new Ray(new Point3D(0.5, 3, 0.5), new Point3D(0, -1, 0)),
                        Box1,
                        2.5
                    },
                new object[]
                    {
                        new Ray(new Point3D(2, 0, 1), new Point3D(-1, 0, 0)),
                        Box1,
                        1
                    },
                new object[]
                    {
                        new Ray(new Point3D(0.5, 5, 0.5), new Point3D(-0.5, -4, -0.5)), //касание вершины
                        Box,
                        Math.Sqrt(33.0 / 2.0)
                    }, 
                new object[]
                    {
                        new Ray(new Point3D(0.5, 5, 0.5), new Point3D(-0.5, -4, 0)), //касание грани
                        Box,
                        Math.Sqrt(65.0 / 4.0)
                    },
                    new object[]
                    {
                        new Ray(new Point3D(0.5, 5, 0.5), new Point3D(0, -4, 0)), //пересечение со стороной
                        Box,
                        4
                    },
                new object[]
                    {
                        new Ray(new Point3D(0.5, 5, 0.5), new Point3D(0, 4, 0)), //нет пересечения 
                        Box,
                        double.PositiveInfinity
                    },
                new object[]
                    {
                        new Ray(new Point3D(0.5, 1, 0.5), new Point3D(0, -1, 0)), //на грани 
                        Box,
                        0
                    }
            };

        [Test]
        [TestCaseSource("_data7")]
        public void RayTest(Ray ray, Box box, double distance)
        {
            var computedDistance = Intersector.IntersectBox(box, ray);
            Assert.AreEqual(distance, computedDistance);
            {
                //Console.WriteLine("Distance:  {0}. expected: {1}", computedDistance, distance);
            }
        }
    }

    [TestFixture]
    internal class GetBoxTest
    {
        private static readonly Angle AAngle = Angle.FromRad(0);
        private static readonly Angle BAngle = Angle.FromRad(0);
        private static readonly Angle CAngle = Angle.FromRad(0/*Math.PI / 3.0*/);
        private static readonly Frame3D Location = new Frame3D(0.5, 0.5, 0, AAngle, BAngle, CAngle);
        private static readonly Box Box = new Box { Location = Location, XSize = 1, YSize = 1, ZSize = 1 };

        private static readonly Frame3D Location1 = new Frame3D(0.5, 0.5, 0.5, new Angle(), new Angle(), Angle.FromGrad(45));
        private static readonly Box Box1 = new Box
        {
            Location = Location1,
            XSize = Math.Sqrt(2),
            YSize = Math.Sqrt(2),
            ZSize = Math.Sqrt(2)
        };

        private readonly object[][] _data8 = new []
            {
                new object[]
                    {
                        Box
                    },
                new object[]
                    {
                            Box1
                    }
            };


        [Test]
        [TestCaseSource("_data8")]
        public void BoxTest(Box box)
        {
            var v = Intersector.GetDirection(box);
            for (int i = 0; i < 3; i++)
            {
               // Console.WriteLine(v[i]);
            }
            var vertex = Intersector.GetVertex(box);
            for (int i = 0; i < vertex.Capacity; i++)
            {
               // Console.WriteLine("next: " + vertex[i]);
            }
        }
    }
}
