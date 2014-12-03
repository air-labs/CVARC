using System;
using NUnit.Framework;
using CVARC.V2;
using System.Runtime.CompilerServices;

namespace Demo.Tests
{
    [TestFixture]
    public class InteractionTests
    {
        void RunTest([CallerMemberName] string testName = "")
        {
            var loader = new Loader();
            loader.AddLevel("Demo", "Interaction", () => new Demo.KroR.Interaction());
            loader.RunSelfTestInVSContext("Demo", "Interaction", testName, new NUnitAsserter());

        }
        [Test]
        public void Grip()
        {
            RunTest();
        }
        [Test]
        public void GripThroughWall()
        {
            RunTest();
        }
        [Test]
        public void Release()
        {
            RunTest();
        }
        [Test]
        public void GripUnGripable()
        {
            RunTest();
        }


    }
}
