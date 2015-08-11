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
        public void LoadPopCornScoringTests(LogicPart logic, RMRules rules)
        {
            AddTest(logic, "Scores_PopCorn_StartingArea", ScoreTest(
                4,
                rules.Move(64),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.Grip(),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-64),
                rules.Stand(0.1),
                rules.Release()
            ));
            
            AddTest(logic, "Scores_PopCorn_UpperCinema", ScoreTest(
                4,
                rules.Move(64),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.Grip(),
                rules.Rotate(Angle.HalfPi),
                rules.Move(130),
                rules.Rotate(Angle.HalfPi),
                rules.Move(40),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(60),
                rules.Stand(0.1),
                rules.Release()
            ));
            
            AddTest(logic, "Scores_PopCorn_BottomCinema", ScoreTest(
                4,
                rules.Move(64),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.Grip(),
                rules.Rotate(Angle.HalfPi),
                rules.Move(130),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(40),
                rules.Rotate(Angle.HalfPi),
                rules.Move(60),
                rules.Stand(0.1),
                rules.Release()
            ));
            
            AddTest(logic, "Scores_PopCorn_BuildingArea", ScoreTest(
                0,
                rules.Move(115),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-60),
                rules.Stand(0.1),
                rules.Grip(),
                rules.Move(-15),
                rules.Stand(0.1),
                rules.Release(),
                rules.Move(40),
                rules.Stand(0.1)
            ));

            AddTest(logic, "Scores_PopCorn_TwoCupSameLocation", ScoreTest(
                4,
                rules.Move(115),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-60),
                rules.Stand(0.1),
                rules.Grip(),
                rules.Move(60),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(-115),
                rules.Stand(0.1),
                rules.Release(),
                rules.Move(64),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.Grip(),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-40),
                rules.Stand(0.1),
                rules.Release()
            ));
            
            AddTest(logic, "Scores_PopCorn_TwoCupDiffLocations", ScoreTest(
                4 + 4,
                rules.Move(115),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-60),
                rules.Stand(0.1),
                rules.Grip(),
                rules.Move(60),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(-115),
                rules.Stand(0.1),
                rules.Release(),
                rules.Move(64),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.Grip(),
                rules.Rotate(Angle.HalfPi),
                rules.Move(130),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(40),
                rules.Rotate(Angle.HalfPi),
                rules.Move(60),
                rules.Stand(0.1),
                rules.Release()
            ));

            AddTest(logic, "Scores_PopCorn_GrippedShouldBeZero", ScoreTest(
                0,
                rules.Move(64),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.Grip(),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-64),
                rules.Stand(0.1)
            ));
        }
    }
}
