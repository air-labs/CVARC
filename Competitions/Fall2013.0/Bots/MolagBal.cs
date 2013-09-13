using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace StarshipRepair.Bots
{
    class MolagBal : FixedProgramBot
    {
        public override void DefineProgram()
        {
            Rot(-90);
            Mov(50);
            Rot(90);
            Mov(120);

            Rot(-90);
            Mov(50);
            Rot(90);
            Mov(50);
            Rot(90);
            Mov(50);
            Rot(-90);

            Mov(120);
            Rot(90);
            Mov(50);
        }
    }
}
