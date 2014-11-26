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
        void Initialize(Competitions competitions, Configuration configuration, ControllerFactory controllerFactory);
        WorldClocks Clocks { get; }
        IdGenerator IdGenerator { get; }
        Scores Scores { get; }
        IEnumerable<IActor> Actors { get; }
        void OnExit();
        event Action Exit;
        Configuration Configuration { get; }
        Competitions Competitions { get; }
        Logger Logger { get; }
        void CreateWorld();
    }

}
