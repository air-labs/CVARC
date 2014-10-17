using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jango
{
    public class Level1 : Competitions
    {
        public Level1()
            : base(new Level1LogicPart(), new KroREnginePart(), new Level1ManagerPart())
        {
        }

    }
}
