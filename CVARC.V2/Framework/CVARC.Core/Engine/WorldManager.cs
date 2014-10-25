using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class WorldManager<TWorld> : IWorldManager
        where TWorld : IWorld
    {
        public TWorld World { get; private set; }

        public virtual void Initialize(IWorld world)
        {
            World = Compatibility.Check<TWorld>(this,world);
        }

        public abstract void CreateWorld(IdGenerator generator);
    }
}
