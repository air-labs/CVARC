using CVARC.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class Level1 : CompetitionsBundle
    {
        public Level1()
            : base(new Gems.Levels.Level1(), new RepairTheStarship.GemsRules())
        { }
    }

    public class Level2 : CompetitionsBundle
    {
        public Level2()
            : base(new Gems.Levels.Level1(), new RepairTheStarship.GemsRules())
        { }
    }


    public class Level3 : CompetitionsBundle
    {
        public Level3()
            : base(new Gems.Levels.Level3(), new RepairTheStarship.GemsRules())
        { }
    }


    public class Level4 : CompetitionsBundle
    {
        public Level4()
            : base(new Gems.Levels.Level4(), new RepairTheStarship.GemsRules())
        { }
    }
}
