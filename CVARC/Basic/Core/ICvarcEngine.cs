using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Core;

namespace CVARC.Basic
{
    public interface ICvarcEngine : IEngine
    {
        Body GetBody(string name);
    }
}
