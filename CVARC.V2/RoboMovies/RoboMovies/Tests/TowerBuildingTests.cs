using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace RoboMovies
{
    partial class RMLogicPartHelper
    {
        [TestLoader]
        public void LoadTowerBuilderTests(LogicPart logic, MoveAndBuildRules rules)
        {
            AddTest(logic, "SimpleTest", TowerBuilderTest(
                0,
                rules.Move(100),
                rules.Stand(1)
            ));

            AddTest(logic, "SimpleScoreTest", ScoreTest(
                0,
                rules.Move(100),
                rules.Stand(1)
            ));
        }
    }
}
