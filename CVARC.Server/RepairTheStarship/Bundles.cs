using CVARC.Basic;

namespace Client
{
    public class Level1 : CompetitionsBundle
    {
        public Level1()
            : base(new RepairTheStarship.Levels.Level1(), new RepairTheStarship.GemsRules())
        { }
    }

    public class Level2 : CompetitionsBundle
    {
        public Level2()
            : base(new RepairTheStarship.Levels.Level2(), new RepairTheStarship.GemsRules())
        { }
    }


    public class Level3 : CompetitionsBundle
    {
        public Level3()
            : base(new RepairTheStarship.Levels.Level3(), new RepairTheStarship.GemsRules())
        { }
    }


    public class Level4 : CompetitionsBundle
    {
        public Level4()
            : base(new RepairTheStarship.Levels.Level4(), new RepairTheStarship.GemsRules())
        { }
    }
}
