using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class ActorManagerFactory<T> : IActorManagerFactory
        where T : IActorManager, new()
    {
        public Type ActorManagerType { get { return typeof(T); } }
        public IActorManager Generate() { return new T(); }
    }
}
