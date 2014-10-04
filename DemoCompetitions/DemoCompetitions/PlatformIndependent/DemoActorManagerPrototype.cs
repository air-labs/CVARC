using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;

namespace DemoCompetitions
{
    abstract class DemoActorManagerPrototype : ActorManager<DemoRobot>
    {
        public abstract void MakeAction();
    }
        
}
