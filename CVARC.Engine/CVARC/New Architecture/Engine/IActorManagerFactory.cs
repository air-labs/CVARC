using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace CVARC.V2
{

    public interface IActorManagerFactory
    {
        Type ActorManagerType { get; }
        IActorManager Generate();
    }


}
