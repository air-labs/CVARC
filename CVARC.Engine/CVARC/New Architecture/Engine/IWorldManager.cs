using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public interface IWorldManager
    {
        void CreateWorld(IWorld world, IdGenerator generator);
    }
}
