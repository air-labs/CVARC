using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public class Engine
    {
        public readonly IdGenerator IdGenerator = new IdGenerator();
        public readonly IPhysical Physical;
        public readonly IWorldManager WorldManager;
        public List<IActorManagerFactory> ActorManagerFactories;

        public Engine(IPhysical physical, IWorldManager worldRules, IEnumerable<IActorManagerFactory> actorManagerFactories)
        {
            Physical = physical;
            WorldManager = worldRules;
            ActorManagerFactories = actorManagerFactories.ToList();
        }
    }
}
