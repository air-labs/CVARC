using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Core;

namespace CVARC.V2
{
    public interface IWorld
    {
        IEngine Engine { get; }
        IWorldManager Manager { get; }
        void Initialize(Competitions competitions, IEnvironment environment);
        WorldClocks Clocks { get; }
        IdGenerator IdGenerator { get; }
        Scores Scores { get; }
    }
}
