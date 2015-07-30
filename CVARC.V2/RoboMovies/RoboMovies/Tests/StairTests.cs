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
            //забираемся почему-то только на жёлтую лесенку
            AddTest(logic, "Scores_MoveToOpponentStair", ScoreTest(50,
                rules.Move(150),
                rules.Rotate(Angle.HalfPi),
                rules.Move(40),
                rules.Stand(0.1),
                rules.UpLadder(),
                rules.Stand(0.1),
                rules.Rotate(Angle.HalfPi),
                rules.Move(50),
                rules.Stand(0.1),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.UpLadder()
            ));
            AddTest(logic, "Scores_UnUsualUp", ScoreTest(50,
                rules.Move(100),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(40),
                rules.Stand(0.1),
                rules.UpLadder(),
                rules.Stand(0.1),
                rules.Move(-50),
                rules.Rotate(Angle.HalfPi),
                rules.Move(50),
                rules.Stand(0.1),
                rules.UpLadder()
            ));
        }
    }
}
