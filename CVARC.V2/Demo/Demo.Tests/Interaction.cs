using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CVARC.V2;
using System.Runtime.CompilerServices;

namespace Demo.Tests
{
    [TestClass]
    public class InteractionTests
    {
        void RunTest([CallerMemberName] string testName = "")
        {
            var loader = new Loader();
            loader.AddLevel("Demo", "Interaction", () => new Demo.KroR.Interaction());
            loader.RunSelfTestInVSContext("Demo", "Interaction", testName, new MSAsserter());

        }
        [TestMethod]
        public void Grip()
        {
            RunTest();
        }
        [TestMethod]
        public void GripThroughWall()
        {
            RunTest();
        }
        [TestMethod]
        public void Release()
        {
            RunTest();
        }
        [TestMethod]
        public void GripUnGripable()
        {
            RunTest();
        }


    }
}
