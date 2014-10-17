using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jango
{
    public class Level1ManagerPart : ManagerPart
    {
        public override IActorManager CreateActorManagerFor(IActor actor)
        {
            return new JangoActorManager();
        }

        public Level1ManagerPart()
            : base(new JangoWorldManager())
        { }
    }
}
