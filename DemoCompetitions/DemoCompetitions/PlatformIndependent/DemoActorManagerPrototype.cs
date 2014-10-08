using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace DemoCompetitions
{
    public interface IDemoActorManagerPrototype  : IActorManager
    {
        void MakeAction();
    }
        
}
