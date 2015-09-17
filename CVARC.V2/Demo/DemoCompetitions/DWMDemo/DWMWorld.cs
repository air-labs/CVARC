using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AIRLab.Mathematics;
using CVARC.V2;

namespace Demo
{
    public class DWMWorld : World<DWMWorldState, IDemoWorldManager>
    {
        public override void CreateWorld()
        {
            Manager.CreateWorld(IdGenerator);
        }

    }
}
