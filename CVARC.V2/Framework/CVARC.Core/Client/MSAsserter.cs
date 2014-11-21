using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class MSAsserter : IAsserter
    {
        public void Assert(double expected, double actual, double delta)
        {
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, actual, delta);
        }
    }
}
