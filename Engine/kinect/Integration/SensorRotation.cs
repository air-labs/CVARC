using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using NUnit.Framework;

namespace kinect.Integration
{
    public class SensorRotation
    {
        public static Point3D GetFrontDirection(Frame3D frame)
        {
            Matrix m = frame.GetMatrix();
            var front = new Point3D(m[0, 0], m[1, 0], m[2, 0]);
            return front;
        }
        /// <summary>
        /// повернуть фрейм вокруг вертикальной оси (z)
        /// </summary>
        /// <param name="frame">положение сонара</param>
        /// <param name="angle">угол поворота</param>
        /// <returns>вектор - новое направление</returns>
        public static Frame3D HorisontalFrameRotation(Frame3D frame, Angle angle)
        {
            Matrix m = frame.GetMatrix();
            //var up = new Point3D(m[0, 2], m[1, 2], m[2, 2]);
            var up = new Point3D(0,0,1);
            Frame3D rotated = frame.Apply(Frame3D.DoRotate(up, angle));
            return rotated;
        }
        public static Point3D HorisontalRotation(Frame3D frame, Angle angle)
        {
            Matrix m = frame.GetMatrix();
            var up = new Point3D(m[0, 2], m[1, 2], m[2, 2]);
            Frame3D rotated = frame.Apply(Frame3D.DoRotate(up, angle));
            Matrix n = rotated.GetMatrix();
            var rotatedFront = new Point3D(n[0, 0], n[1, 0], n[2, 0]);
            return rotatedFront;
        }
        /// <summary>
        /// повернуть фрейм вокруг горизонтальной оси (y)
        /// </summary>
        /// <param name="frame">положение сонара</param>
        /// <param name="angle">угол поворота</param>
        /// <returns>вектор - новое направление</returns>
        public static Frame3D VerticalFrameRotation(Frame3D frame, Angle angle)
        {
            Matrix m = frame.GetMatrix();
            //var right = new Point3D(m[0, 1], m[1, 1], m[2, 1]);
            var right = new Point3D(0,-1,0);
            Frame3D rotated = frame.Apply(Frame3D.DoRotate(right, angle));
            return rotated;
        }
        public static Point3D VerticalRotation(Frame3D frame, Angle angle)
        {
            Matrix m = frame.GetMatrix();
            var right = new Point3D(m[0, 1], m[1, 1], m[2, 1]);
            Frame3D rotated = frame.Apply(Frame3D.DoRotate(right, angle));
            Matrix n = rotated.GetMatrix();
            var rotatedFront = new Point3D(n[0, 0], n[1, 0], n[2, 0]);
            return rotatedFront;
        }
    }

    #region RotationTest
    [TestFixture]
    internal class FullRotationTest
    {
        private static readonly Frame3D Frame = new Frame3D();

        private readonly object[][] hor_data = new[]
            {
                new object[]
                    {
                        Frame,
                        Angle.FromRad(Math.PI/2.0),
                        new Point3D(0, 1, 0)
                    },
                    new object[]
                    {
                        Frame,
                        Angle.FromRad(-Math.PI/2.0),
                        new Point3D(0, -1, 0)
                    },
                    new object[]
                    {
                        Frame,
                        Angle.FromRad(Math.PI/4.0),
                        new Point3D(Math.Sqrt(2)/2.0, Math.Sqrt(2)/2.0, 0)
                    },
                    new object[]
                    {
                        Frame,
                        Angle.FromRad(0),
                        new Point3D(1, 0, 0)
                    },
                    new object[]
                    {
                        Frame,
                        Angle.FromRad(Math.PI/6.0),
                        new Point3D(Math.Sqrt(3.0)/2.0, 0.5, 0)
                    },
                    new object[]
                    {
                        Frame,
                        Angle.FromRad(Math.PI/3.0),
                        new Point3D(0.5, Math.Sqrt(3.0)/2.0, 0)
                    }
            };

        private readonly object[][] ver_data = new[]
            {
                new object[]
                    {
                        Frame,
                        Angle.FromRad(Math.PI/2.0),
                        new Point3D(0, 0, -1)
                    },
                    new object[]
                    {
                        Frame,
                        Angle.FromRad(-Math.PI/2.0),
                        new Point3D(0, 0, 1)
                    },
                    new object[]
                    {
                        Frame,
                        Angle.FromRad(Math.PI/4.0),
                        new Point3D(Math.Sqrt(2)/2.0, 0, -Math.Sqrt(2)/2.0)
                    },
                    new object[]
                    {
                        Frame,
                        Angle.FromRad(0),
                        new Point3D(1, 0, 0)
                    },
                    new object[]
                    {
                        Frame,
                        Angle.FromRad(Math.PI/3.0),
                        new Point3D(0.5, 0, -Math.Sqrt(3.0)/2.0)
                    }
            };

        [Test]
        [TestCaseSource("hor_data")]
        public void HorizontalTest(Frame3D frame, Angle angle, Point3D rotated)
        {
            Point3D computed = SensorRotation.HorisontalRotation(frame, angle);
            Assert.AreEqual(rotated, computed);
            Console.WriteLine("Expected: {0}. But was: {1}", rotated, computed);

        }

        [Test]
        [TestCaseSource("ver_data")]
        public void VerticalTest(Frame3D frame, Angle angle, Point3D rotated)
        {
            Point3D computed = SensorRotation.VerticalRotation(frame, angle);
            Assert.AreEqual(rotated, computed);
            Console.WriteLine("Expected: {0}. But was: {1}", rotated, computed);

        }
    }
    #endregion RotationTest
}
