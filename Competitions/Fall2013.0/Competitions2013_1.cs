using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace Gems
{
    public class Competitions2013_1 : Competitions
    {
        public Competitions2013_1()
            : base(new GemsWorld(), new Behaviour(), new KbController())
        { }
    }
}
