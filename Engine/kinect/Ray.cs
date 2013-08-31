using System;
using AIRLab.Mathematics;
using NUnit.Framework;

namespace kinect
{
    public class Ray
	{
        public Point3D Origin { get; private set; }
        public Point3D Direction { get; private set; }
		//Конструктор луча по началу и направлению
		public Ray(Point3D origin, Point3D direction)
		{
			Origin = new Point3D(origin.X, origin.Y, origin.Z);
		    Direction = new Point3D(direction.X, direction.Y, direction.Z);
		    Direction = Direction.Normalize();
		}
		//Получить точку прямой в зависимости от параметра t
		public Point3D CalculatePoint(double t)
		{
			double x = Origin.X + t * Direction.X;
			double y = Origin.Y + t * Direction.Y;
			double z = Origin.Z + t * Direction.Z;
			return new Point3D(x, y, z);
		}
		//Получить длину луча от начала до точки с параметром t
		public double DistanceTo(double t)
		{
			var point = CalculatePoint(t);
		    var vec = Origin - point;
		    return Math.Sqrt(vec.X*vec.X + vec.Y*vec.Y + vec.Z*vec.Z);
		}
        public override string ToString()
        {
            var s = string.Format("origin point: {0} direction: {1}", Origin, Direction);
            return s;
        }
	}

    [TestFixture]
    internal class RayTester
    {
        private readonly object[][] _data = new []
            {
                new object[]
                    {
                        new Ray(new Point3D(0, 0, 0), new Point3D(1.0/3.0, 1.0/3.0, 1.0/3.0)),
                        new Point3D(Math.Sqrt(1.0/3.0), Math.Sqrt(1.0/3.0), Math.Sqrt(1.0/3.0)),
                        1.0
                    }
            };


        [Test]
        [TestCaseSource("_data")]
        public void RayTest(Ray ray, Point3D point, double distance)
        {
            {
                Assert.AreEqual(ray.Origin, new Point3D(0, 0, 0));
                Assert.AreEqual(ray.Direction, point);
                Console.WriteLine("Ray: " + ray);
            }
        }
    }

    internal class Programm
    {
        public static void Main()
        {
            
        }
    }
}