using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface ICvarcTest
    {
        void Run(int port, IWorldHolder holder, IAsserter asserter);
    }
}
