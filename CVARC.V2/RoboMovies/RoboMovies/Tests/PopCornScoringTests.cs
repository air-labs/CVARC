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
            AddTest(logic, "PopCorn_Scores_StartingArea", ScoreTest(
                4,
                rules.Move(64),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.Grip(),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-54),
                rules.Stand(0.1),
                rules.Release()
            ));
            
            AddTest(logic, "PopCorn_Scores_UpperCinema", ScoreTest(
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
            
            AddTest(logic, "PopCorn_Scores_BottomCinema", ScoreTest(
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
            
            AddTest(logic, "PopCorn_Scores_BuildingArea", ScoreTest(
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

            AddTest(logic, "PopCorn_Scores_TwoCupsSameLocation", ScoreTest(
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
            
            AddTest(logic, "PopCorn_Scores_TwoCupsDiffLocations", ScoreTest(
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

            AddTest(logic, "PopCorn_Scores_GrippedShouldBeZero", ScoreTest(
                0,
                rules.Move(64),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.Grip(),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-64),
                rules.Stand(0.1)
            ));

            AddTest(logic, "PopCorn_Scores_FilledCup", ScoreTest(
                5,
                rules.Move(64),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.Grip(),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-30),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(-70),
                rules.Stand(0.1),
                rules.GetPopCorn(),
                rules.Move(70),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-10),
                rules.Stand(0.1),
                rules.Release()
            ));

            AddTest(logic, "PopCorn_Scores_TwoCupsFilled", ScoreTest(
                5,
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
                rules.Move(-30),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(-70),
                rules.Stand(0.1),
                rules.GetPopCorn(),
                rules.Move(70),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-10),
                rules.Stand(0.1),
                rules.Release()
            ));
        }
    }
}
