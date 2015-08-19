using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using AIRLab.Mathematics;

namespace RoboMovies
{
    partial class RMLogicPartHelper
    {
        [TestLoaderMethod]
        public void LoadStairTests(LogicPart logic, RMRules rules)
        {
            var yellowUp = new RMCommand[] {
                rules.Move(90),
                rules.Rotate(Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.UpLadder(),
            };

            var greenUp = new RMCommand[] {
                rules.Move(140),
                rules.Rotate(Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.UpLadder(),
            };

            AddTest(logic, "Stairs_YellowSuccess", PositionTest(-25, 80, 10, yellowUp));
            AddTest(logic, "Stairs_GreenFailure", PositionTest(25, 30, 10, greenUp));
            
            AddTest(logic, "Stairs_Scoring_Success", ScoreTest(50, yellowUp));
            AddTest(logic, "Stairs_Scoring_Failure", ScoreTest(0, greenUp));
            
            AddTest(logic, "Stairs_Scoring_UpWithoutCommand", ScoreTest(0, 
                rules.Move(90),
                rules.Rotate(Angle.HalfPi),
                rules.Move(50),
                rules.Stand(0.1)
            ));
        }
    }
}
