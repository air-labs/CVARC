using System;
using AIRLab.Mathematics;
using CVARC.Core;
using NUnit.Framework;

namespace kinect
{
    public partial class Intersector
    {
        public static double IntersectBall(Ball ball, Ray ray)
        {
            //Переменные для возврата корней квадратного уравнения
            double root1, root2;
            //Расчет коэффициентов решения системы уравнений сферы и луча
            var a = ray.Direction.MultiplyScalar(ray.Direction);
            //Вспомогательный вектор
            var location = new Point3D(ball.GetAbsoluteLocation().X, ball.GetAbsoluteLocation().Y, ball.GetAbsoluteLocation().Z + ball.Radius);
            var vec = ray.Origin - location;
            var b = 2 * ray.Direction.MultiplyScalar(vec);
            var c = vec.MultiplyScalar(vec) - Math.Pow(ball.Radius, 2);
            //Обработка решения
            if ((QuadEquation.Solution(a, b, c, out root1, out root2)) && ((root1 >= 0) | (root2 >= 0)))
                if (root1 >= 0)
                {
                    return ray.DistanceTo(root1);
                }
                else
                {
                    return ray.DistanceTo(root2);
                }
            return Double.PositiveInfinity;
        }
    }
    [TestFixture]
    internal class BallIntersectionTest
    {
        private static readonly Ball Ball1 = new Ball { Radius = 1 };
        private static readonly Frame3D Location = new Frame3D(3, 3, 1);
        private static readonly Ball Ball2 = new Ball { Location = Location, Radius = 2 };

        private readonly object[][] _data = new []
            {
                new object[]
                    {
                        //Параллельные OY лучи
                        new Ray(new Point3D(0, 2, 1), new Point3D(0, -1, 0)),
                        Ball1,
                        1.0
                    },
                new object[]
                    {
                        //Параллельные OY лучи
                        new Ray(new Point3D(0, 2, 2), new Point3D(0, -1, 0)),
                        Ball1,
                        2.0
                    },
                new object[]
                    {   
                        //Параллельные OY лучи
                        new Ray(new Point3D(0, 2, 3), new Point3D(0, -1, 0)),
                        Ball1,
                        double.PositiveInfinity
                    },
                new object[]
                    {
                        //Параллельные OY лучи
                        new Ray(new Point3D(0, 2, 1), new Point3D(0, 1, 0)),
                        Ball1,
                        double.PositiveInfinity
                    },
                new object[]
                    {
                        //Другой шар, луч не параллелен осям
                        new Ray(new Point3D(0, 0, 0), new Point3D(3, 3, 1)),
                        Ball2,
                        Math.Sqrt(19)
                    },
                new object[]
                    {
                        //Луч не параллелен осям
                        new Ray(new Point3D(2, 6, 1), new Point3D(1, -1, 2)),
                        Ball2,
                        Math.Sqrt(6)
                    },
                new object[]
                    {
                        //Параллельные OZ лучи
                        new Ray(new Point3D(3, 3, 0), new Point3D(0, 0, 1)),
                        Ball2,
                        1
                    },
                new object[]
                    {
                        //Параллельные OZ лучи
                        new Ray(new Point3D(3, 1, 0), new Point3D(0, 0, 1)),
                        Ball2,
                        3
                    }
            };


        [Test]
        [TestCaseSource("_data")]
        public void RayTest(Ray ray, Ball ball, double distance)
        {
            var computedDistance = Intersector.IntersectBall(ball, ray);
            Assert.AreEqual(distance, computedDistance);
            {
                //Console.WriteLine("Distance:  {0}. Expected: {1}", computedDistance, distance);
            }
        }
    }
}
