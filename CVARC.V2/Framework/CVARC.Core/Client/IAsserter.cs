using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface IAsserter
    {
        public void IsEqual(double expected, double actual, double delta);
    }
}
