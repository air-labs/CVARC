using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface IWorld
    {
        IEngine Engine { get; }
        IWorldManager Manager { get; }
        void Initialize(Competitions competitions, Environment environment);
        WorldClocks Clocks { get; }
    }
}
