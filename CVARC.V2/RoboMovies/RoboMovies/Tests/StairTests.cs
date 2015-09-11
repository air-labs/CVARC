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

            var greenUp = new RMCommand[yellowUp.Length];
            yellowUp.CopyTo(greenUp, 0);
            greenUp[0] = rules.Move(140);

            AddTest(logic, "Stairs_YellowSuccess", PositionTest(-25, 80, 10, yellowUp));
            AddTest(logic, "Stairs_GreenFailure", PositionTest(25, 30, 10, greenUp));
            
            AddTest(logic, "Stairs_Scoring_Success", ScoreTest(15, yellowUp));
            AddTest(logic, "Stairs_Scoring_Failure", ScoreTest(0, greenUp));
            
            AddTest(logic, "Stairs_Scoring_UpWithoutCommand", ScoreTest(0, 
                rules.Move(90),
                rules.Rotate(Angle.HalfPi),
                rules.Move(80),
                rules.Stand(0.1)
            ));
        }
    }
}
