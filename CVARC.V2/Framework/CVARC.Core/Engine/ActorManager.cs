using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class ActorManager<TActor> : IActorManager
       where TActor : IActor
    {
        public TActor Actor { get; private set; }

        IActor IActorManager.Actor
        {
            get { return Actor; }
        }

        public void Initialize(IActor actor)
        {
            Actor = Compatibility.Check<TActor>(this, actor);
        }

        public abstract void CreateActorBody();
    }
}
