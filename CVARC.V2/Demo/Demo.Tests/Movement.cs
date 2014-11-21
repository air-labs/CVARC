using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CVARC.V2;

namespace Demo.Tests
{
    [TestClass]
    public class UnitTest1
    {
        void RunTest(string assemblyName, string level, string testName)
        {
            var loader = new Loader();
            loader.AddLevel("Demo", "Movement", () => new Demo.KroR.Movement());
            var world = loader.LoadSelfTest(assemblyName, level, testName, new MSAsserter());
            world.RunWithoutInterface();
        }

        [TestMethod]
        public void Forward()
        {
            RunTest("Demo", "Movement", "Forward");
        }
    }
}
