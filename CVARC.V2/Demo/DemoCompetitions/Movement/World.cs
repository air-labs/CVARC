using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AIRLab.Mathematics;
using CVARC.V2;

namespace Demo
{
    public class MovementWorld : World<MovementWorldState, IWorldManager>
    {


        public override void CreateWorld()
        {
           Manager.CreateWorld(IdGenerator);
        }

    }
}
