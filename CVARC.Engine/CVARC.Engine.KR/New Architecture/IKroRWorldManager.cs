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

        public KroRWorldManager()
        {
            Root=new Body();
        }
        
        public Body Root
        {
            get;
            private set; 
        }
    }
}
