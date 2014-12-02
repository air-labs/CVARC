using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CVARC.V2;
using System.Runtime.CompilerServices;

namespace Demo.Tests
{
    [TestClass]
    public class MovementTests
    {
        void RunTest([CallerMemberName] string testName="")
        {
            var loader = new Loader();
            loader.AddLevel("Demo", "Movement", () => new Demo.KroR.Movement());
            loader.RunSelfTestInVSContext("Demo", "Movement", testName, new MSAsserter()); ;
        }

        [TestMethod]
        public void Forward()
        {
            RunTest();
        }

        [TestMethod]
        public void Backward()
        {
            RunTest();
        }

        [TestMethod]
        public void Exit()
        {
            RunTest();
        }

        [TestMethod]
        public void Square()
        {
            RunTest();
        }
        [TestMethod]
        public void RotateRect()
        {
            RunTest();
        }
        [TestMethod]
        public void ForwardRect()
        {
            RunTest();
        }

        [TestMethod]
        public void BackwardRect()
        {
            RunTest();
        }

        [TestMethod]
        public void SquareRect()
        {
            RunTest();
        }
        [TestMethod]
        public void Rotate()
        {
            RunTest();
        }
        [TestMethod]
        public void AlignmentRect()
        {
            RunTest();
        }
        [TestMethod]
        public void Speed()
        {
            RunTest();
        }
        [TestMethod]
        public void SpeedRect()
        {
            RunTest();
        }
        [TestMethod]
        public void RotateSpeed()
        {
            RunTest();
        }
        [TestMethod]
        public void RotateSpeedRect()
        {
            RunTest();
        }
        [TestMethod]
        public void FuckTheBoxRect()
        {
            RunTest();
        }
        [TestMethod]
        public void FuckTheBox()
        {
            RunTest();
        }
        public static void Main()
        {
            new MovementTests().Backward();
        }

    }
}
