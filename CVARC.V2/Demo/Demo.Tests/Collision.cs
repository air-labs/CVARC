using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CVARC.V2;
using System.Runtime.CompilerServices;

namespace Demo.Tests
{
    [TestClass]
    public class CollisionTests
    {
        void RunTest([CallerMemberName] string testName = "")
        {
            var loader = new Loader();
            loader.AddLevel("Demo", "Collision", () => new Demo.KroR.Collision());
            loader.RunSelfTestInVSContext("Demo", "Collision", testName, new MSAsserter());
        }
        [TestMethod]
        public void CollisionCount()
        {
            RunTest();
        }
        [TestMethod]
        public void NoCollision()
        {
            RunTest();
        }
        [TestMethod]
        public void CollisionBox()
        {
            RunTest();
        }
        [TestMethod]
        public void NoBox()
        {
            RunTest();
        }
        [TestMethod]
        public void RotateNoBox()
        {
            RunTest();
        }
        [TestMethod]
        public void RotateBox()
        {
            RunTest();
        }

    }
}
