using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Core;

namespace CVARC.V2
{
    public interface IKroRWorldManager
    {
        Body Root { get; }
    }

    public abstract class KroRWorldManager<TWorld> : WorldManager<TWorld>, IKroRWorldManager
        where TWorld : IWorld
    {
        public KroREngine Engine { get { return (KroREngine)World.Engine; } }
        public Body Root { get { return Engine.Root; } }
    }
}
