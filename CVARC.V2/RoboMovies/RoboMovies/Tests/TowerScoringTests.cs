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
        public void LoadTowerScoringTests(LogicPart logic, RMRules rules)
        {
            AddTest(logic, "Scores_Zero", ScoreTest(
                0,
                rules.Move(65),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.BuildTower()
            ));
            
            AddTest(logic, "Scores_Bottom_Building_Area", ScoreTest(
                2,
                rules.Move(100),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(50),
                rules.Stand(0.1),
                rules.BuildTower()
            ));
            
            AddTest(logic, "Scores_Starting_Area_Square", ScoreTest(
                2,
                rules.Move(65),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(-25),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(65),
                rules.Stand(0.1),
                rules.BuildTower()
            ));
            
            AddTest(logic, "Scores_Starting_Area_Circle", ScoreTest(
                2,
                rules.Move(65),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(-25),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(35),
                rules.Stand(0.1),
                rules.BuildTower()
            ));
            
            AddTest(logic, "Scores_Building_In_Yellow_Square", ScoreTest(
                0,
                rules.Move(65),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(20),
                rules.Rotate(Angle.HalfPi),
                rules.Move(170),
                rules.Stand(0.1),
                rules.BuildTower()
            ));

            AddTest(logic, "Scores_Big_Tower", ScoreTest(
                2 * 2,
                rules.Move(65),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(15),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Stand(1),
                rules.Collect(),
                rules.Move(20),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(30),
                rules.Stand(0.1),
                rules.BuildTower()
            ));
            
            AddTest(logic, "Scores_Tower_With_Light", ScoreTest(
                2 + 3,
                rules.Rotate(Angle.Pi),
                rules.Move(10),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Rotate(Angle.Pi),
                rules.Move(105),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(50),
                rules.Stand(0.1),
                rules.BuildTower()
            ));
            
            /* FIXME: не хватает времени на тест! Как увеличить время? о_О */
            AddTest(logic, "Scores_Big_Tower_With_Light", ScoreTest(
                (2 + 3) * 2,
                rules.Rotate(Angle.Pi),
                rules.Move(10),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Rotate(Angle.Pi),
                rules.Move(75),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(15),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Stand(1),
                rules.Collect(),
                rules.Move(20),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(30),
                rules.Stand(0.1),
                rules.BuildTower()
            ));

            AddTest(logic, "Scores_Light_Without_Stands", ScoreTest(
                0,
                rules.Rotate(Angle.Pi),
                rules.Move(10),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(-10),
                rules.Stand(0.1),
                rules.BuildTower()
            ));
            
            AddTest(logic, "Scores_Scam_Test", ScoreTest(
                2,
                rules.Move(100),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(50),
                rules.Stand(0.1),
                rules.BuildTower(),
                rules.Collect(),
                rules.BuildTower(),
                rules.Collect(),
                rules.BuildTower(),
                rules.Collect(),
                rules.BuildTower()
            ));
            
            AddTest(logic, "Scores_Invalid_Stand_Penalty", ScoreTest(
                -10,
                rules.Move(135),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect()
            ));
        }
    }
}
