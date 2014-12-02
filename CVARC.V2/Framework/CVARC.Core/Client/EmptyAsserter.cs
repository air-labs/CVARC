using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class EmptyAsserter : IAsserter
    {
        public void IsEqual(double expected, double actual, double delta)
        {
            
        }
        public void IsEqual(bool expected, bool actual)
        {

        }
    }
}
