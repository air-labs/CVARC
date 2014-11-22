using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;

namespace Demo
{
    public class MovementManagerPart : ManagerPart 
    {
        public MovementManagerPart()
            : base(() => new MovementWorldManager())
        { }

        public override IActorManager CreateActorManagerFor(IActor actor)
        {
            return new ActorManager();
        }
    }
}
