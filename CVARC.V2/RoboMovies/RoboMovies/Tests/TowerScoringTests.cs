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
            
            AddTest(logic, "Scores_BottomBuildingArea", ScoreTest(
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
            
            AddTest(logic, "Scores_StartingAreaSquare", ScoreTest(
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
            
            AddTest(logic, "Scores_StartingAreaCircle", ScoreTest(
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
            
            AddTest(logic, "Scores_BuildingInYellowSquare", ScoreTest(
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

            AddTest(logic, "Scores_BigTower", ScoreTest(
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
            
            AddTest(logic, "Scores_TowerWithLight", ScoreTest(
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
            
            AddTest(logic, "Scores_BigTowerWithLight", ScoreTest(
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
