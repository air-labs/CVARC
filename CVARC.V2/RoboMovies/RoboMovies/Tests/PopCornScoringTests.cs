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
            AddTest(logic, "Scores_PopCornStartingArea", ScoreTest(
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
            
            AddTest(logic, "Scores_PopCornUpperCinema", ScoreTest(
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
            
            AddTest(logic, "Scores_PopCornBottomCinema", ScoreTest(
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
            
            AddTest(logic, "Scores_PopCornBuildingArea", ScoreTest(
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

            AddTest(logic, "Scores_TwoPopCornOneLocation", ScoreTest(
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
            
            AddTest(logic, "Scores_TwoPopCornTwoLocations", ScoreTest(
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

            AddTest(logic, "Scores_GrippedPopCornShouldBeZero", ScoreTest(
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
