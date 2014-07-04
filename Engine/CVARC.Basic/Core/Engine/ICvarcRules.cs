using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Core;

namespace CVARC.Basic
{
    public interface ICvarcRules
    {
        Body CreateWorld(ISceneSettings settings);
        void PerformAction(CVARCEngine engine, string actor, string action);
    }
}
