using System;
using CVARC.V2;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace Demo.Tests
{
    [TestFixture]
    public class CollisionTests
    {
        void RunTest([CallerMemberName] string testName = "")
        {
            var loader = new Loader();
            loader.AddLevel("Demo", "Collision", () => new Demo.KroR.Collision());
            loader.RunSelfTestInVSContext("Demo", "Collision", testName, new NUnitAsserter());
        }
        [Test]
        public void CollisionCount()
        {
            RunTest();
        }
        [Test]
        public void NoCollision()
        {
            RunTest();
        }
        [Test]
        public void CollisionBox()
        {
            RunTest();
        }
        [Test]
        public void NoBox()
        {
            RunTest();
        }
        [Test]
        public void RotateNoBox()
        {
            RunTest();
        }
        [Test]
        public void RotateBox()
        {
            RunTest();
        }

    }
}
