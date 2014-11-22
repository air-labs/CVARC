using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;

namespace Demo.KroR
{
    public class CollisionManagerPart : ManagerPart
    {
        public CollisionManagerPart()
            : base(new CollisionWorldManager())
        { }

        public override IActorManager CreateActorManagerFor(IActor actor)
        {
            return new CollisionActorManager();
        }
    }
}
