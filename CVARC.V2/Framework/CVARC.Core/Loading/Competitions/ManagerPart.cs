using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class ManagerPart
    {
        public readonly Func<IWorldManager> WorldManagerFactory;

        public abstract IActorManager CreateActorManagerFor(IActor actor);

        public ManagerPart(Func<IWorldManager> worldManagerFactory)
        {
            WorldManagerFactory = worldManagerFactory;
        }
    }
}
