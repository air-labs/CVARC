using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;

namespace Demo
{
    public class InteractionManagerPart : ManagerPart 
    {
        public InteractionManagerPart()
            : base(new InteractionWorldManager())
        { }

        public override IActorManager CreateActorManagerFor(IActor actor)
        {
            return new ActorManager();
        }
    }
}
