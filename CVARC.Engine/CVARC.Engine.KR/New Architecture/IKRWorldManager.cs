using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Core;

namespace CVARC.V2
{
    public interface IKRWorldManager
    {
        Body Root { get; }
    }

    public abstract class KRWorldManager<TWorld> : WorldManager<TWorld>, IKRWorldManager
        where TWorld : IWorld
    {

        public KRWorldManager()
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
