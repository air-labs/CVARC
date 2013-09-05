using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace Gems
{
    public class SimpleBot : FixedProgramBot
    {
        public override void DefineProgram()
        {
 	        Rot(-90);
            Mov(50);
            Cmd("Grip");
            Mov(-50);
            Rot(90);
            Mov(20);
            Cmd("Release");
        }
    }
}
