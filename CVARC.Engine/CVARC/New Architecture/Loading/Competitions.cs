using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class Competitions
    {
        public readonly IWorld World;
        public readonly IWorldManager WorldManager;
        public readonly IEnumerable<IActorManagerFactory> ActorManagerFactories;
        public readonly IEngine Engine;

        public Competitions(IWorld world, IEngine Engine, IWorldManager manager, IEnumerable<IActorManagerFactory> actorManagerFactories)
        {
            this.World = world;
            this.Engine = Engine;
            this.WorldManager = manager;
            this.ActorManagerFactories = actorManagerFactories.ToList();
        }
    }
}
