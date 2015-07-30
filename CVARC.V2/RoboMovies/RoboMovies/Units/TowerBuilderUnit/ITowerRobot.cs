using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface ITowerBuilderRobot : IActor 
    {
        TowerBuilderUnit TowerBuilder { get; }
    }
}
