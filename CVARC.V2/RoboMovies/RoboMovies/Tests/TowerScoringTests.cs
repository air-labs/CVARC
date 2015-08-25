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
            AddTest(logic, "Tower_Scores_Zero", ScoreTest(
                0,
                rules.Move(65),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.BuildTower()
            ));
            
            AddTest(logic, "Tower_Scores_BottomBuildingArea", ScoreTest(
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
            
            AddTest(logic, "Tower_Scores_StartingAreaSquare", ScoreTest(
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
            
            AddTest(logic, "Tower_Scores_StartingAreaCircle", ScoreTest(
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
            
            AddTest(logic, "Tower_Scores_BuildingInYellowSquare", ScoreTest(
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

            AddTest(logic, "Tower_Scores_BigTower", ScoreTest(
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
            
            AddTest(logic, "Tower_Scores_TowerWithLight", ScoreTest(
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
            
            AddTest(logic, "Tower_Scores_BigTowerWithLight", ScoreTest(
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

            AddTest(logic, "Tower_Scores_LightWithoutStands", ScoreTest(
                0,
                rules.Rotate(Angle.Pi),
                rules.Move(10),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(-10),
                rules.Stand(0.1),
                rules.BuildTower()
            ));
            
            AddTest(logic, "Tower_Scores_ScamTest", ScoreTest(
                2,
                rules.Move(100),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(-25),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(90),
                rules.Stand(0.1),
                rules.BuildTower(),
                rules.Collect(),
                rules.BuildTower(),
                rules.Collect(),
                rules.BuildTower(),
                rules.Collect(),
                rules.BuildTower()
            ));
            
            AddTest(logic, "Tower_Scores_InvalidStandPenalty", ScoreTest(
                -10,
                rules.Move(135),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect()
            ));

            AddTest(logic, "Tower_Scores_TwoTowersOneLocation", ScoreTest(
                (2 + 3) * 2 /* 2-level spotlight with bonus */ + (2 + 0) * 1 /* one level spotlight, no bonus */,
                rules.Rotate(Angle.Pi),
                rules.Move(10),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(-74),
                rules.Rotate(Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(-25),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(44),
                rules.Stand(0.1),
                rules.BuildTower(),
                rules.Move(-10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(85),
                rules.Rotate(Angle.HalfPi),
                rules.Move(50),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(30),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Rotate(Angle.HalfPi),
                rules.Move(35),
                rules.Rotate(Angle.HalfPi),
                rules.Move(50),
                rules.Stand(0.1),
                rules.BuildTower()
            ));
            
            AddTest(logic, "Tower_Scores_TwoTowersTwoLocations", ScoreTest(
                (2 + 3) * 2 + (2 + 3) * 1,
                rules.Rotate(Angle.Pi),
                rules.Move(10),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(-74),
                rules.Rotate(Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(-25),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(44),
                rules.Stand(0.1),
                rules.BuildTower(),
                rules.Move(-10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(85),
                rules.Rotate(Angle.HalfPi),
                rules.Move(50),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(30),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(10),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.BuildTower()
            ));
        }
    }
}
