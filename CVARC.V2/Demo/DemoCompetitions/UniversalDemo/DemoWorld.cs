using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AIRLab.Mathematics;
using CVARC.V2;

namespace Demo
{
    public class DemoWorld : World<DemoWorldState, IDemoWorldManager>
    {


        public override void CreateWorld()
        {
            Manager.CreateWorld(IdGenerator);
            foreach (var obj in WorldState.Objects)
                Manager.CreateObject(obj);
        }

    }
}
