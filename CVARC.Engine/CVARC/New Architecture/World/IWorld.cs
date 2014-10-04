using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface IWorld
    {
        void Tick(double time);
        IEngine Engine { get; }
        IWorldManager Manager { get; }
        void Initialize(Competitions competitions, CompetitionsEnvironment environment);
    }
}
