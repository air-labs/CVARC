using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface IAsserter
    {
        void IsEqual(double expected, double actual, double delta);
    }
}
