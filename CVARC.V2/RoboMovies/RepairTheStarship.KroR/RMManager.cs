using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;

namespace RoboMovies.KroR
{
    public class RMManagerPart : ManagerPart
    {
        public RMManagerPart() : base(()=>new RMWorldManager())
        {

        }

        public override IActorManager CreateActorManagerFor(IActor actor)
        {
            return new RMActorManager();
        }
    }
}
