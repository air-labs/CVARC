using System;
using CVARC.V2;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using System.Threading;

namespace Demo.Tests
{
    public class NUnitAsserter : IAsserter
    {

        public void IsEqual(double expected, double actual, double delta)
        {
            Assert.AreEqual(expected, actual, delta);
        }

        public void IsEqual(bool expected, bool actual)
        {
            Assert.AreEqual(expected, actual);
        }
    }

    [TestFixture]
    public class MovementTests
    {

        void RunTest([CallerMemberName] string testName="")
        {
			Console.WriteLine("Enter to test  " + testName + " at " + DateTime.Now);
            var loader = new Loader();
            loader.AddLevel("Demo", "Movement", () => new Demo.KroR.Movement());
			try
			{
				loader.RunSelfTestInVSContext("Demo", "Movement", testName, new NUnitAsserter());
			}
			catch (Exception e)
			{
				throw e;
			}
			finally
			{
				Console.WriteLine("Exit from test " + testName + " at " + DateTime.Now);
			}
        }

        [Test]
        public void Forward()
        {
            RunTest();
        }

        [Test]
        public void Backward()
        {
            RunTest();
        }

     

        [Test]
        public void Square()
        {
            RunTest();
        }
        [Test]
        public void RotateRect()
        {
            RunTest();
        }
        [Test]
        public void ForwardRect()
        {
            RunTest();
        }

        [Test]
        public void BackwardRect()
        {
            RunTest();
        }

        [Test]
        public void SquareRect()
        {
            RunTest();
        }
        [Test]
        public void Rotate()
        {
            RunTest();
        }
        [Test]
        public void AlignmentRect()
        {
            RunTest();
        }
        [Test]
        public void Speed()
        {
            RunTest();
        }
        [Test]
        public void SpeedRect()
        {
            RunTest();
        }
        [Test]
        public void RotateSpeed()
        {
            RunTest();
        }
        [Test]
        public void RotateSpeedRect()
        {
            RunTest();
        }
        [Test]
        public void FuckTheBoxRect()
        {
            RunTest();
        }
        [Test]
        public void FuckTheBox()
        {
            RunTest();
        }
      

    }
}
