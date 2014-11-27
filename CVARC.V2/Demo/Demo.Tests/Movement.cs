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


    }
}
