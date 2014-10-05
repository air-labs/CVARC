using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;

namespace RepairTheStarship.KroR
{
    public class RTSManagerPart : ManagerPart
    {
        public RTSManagerPart()
            : base(
                new RTSWorldManager(),
                new IActorManagerFactory[] { })
        { }
    }
}
