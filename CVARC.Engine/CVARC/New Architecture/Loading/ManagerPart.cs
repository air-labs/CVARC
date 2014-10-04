using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class ManagerPart
    {
        public readonly IWorldManager WorldManager;
        public readonly IEnumerable<IActorManagerFactory> ActorManagerFactories;
        public ManagerPart(IWorldManager worldManager, IEnumerable<IActorManagerFactory> actorManagerFactories)
        {
            WorldManager = worldManager;
            ActorManagerFactories = actorManagerFactories;
        }
    }
}
