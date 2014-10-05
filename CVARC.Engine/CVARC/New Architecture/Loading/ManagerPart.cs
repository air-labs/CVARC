using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class ManagerPart
    {
        public readonly IWorldManager WorldManager;

        public abstract IActorManager CreateActorManagerFor(IActor actor);

        public ManagerPart(IWorldManager worldManager)
        {
            WorldManager = worldManager;
        }
    }
}
