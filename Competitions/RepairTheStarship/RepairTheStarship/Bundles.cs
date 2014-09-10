using CVARC.Basic.Core;using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gems
{
    public class Level1 : CompetitionsBundle
    {
        public Level1()
            : base(new RepairTheStarship.Level1(), new RepairTheStarship.GemsRules())
        { }
    }
    public class Level2 : CompetitionsBundle
    {
        public Level2()
            : base(new RepairTheStarship.Level2(), new RepairTheStarship.GemsRules())
        { }
    }

}
