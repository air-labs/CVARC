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

    public class ActorRulesFactory<T> : IActorManagerFactory
        where T : IActorManager
    {
        public Type ActorManagerType { get { return typeof(T); } }
        public Func<T> Generator { get; private set; }
        public IActorManager Generate() { return Generator(); }
        public ActorRulesFactory(Func<T> generator)
        {
            Generator = generator;
        }

    }
}
