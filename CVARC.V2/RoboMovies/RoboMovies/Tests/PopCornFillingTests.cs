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
        public void LoadPopCornFillingTests(LogicPart logic, RMRules rules)
        {
            AddTest(logic, "PopCorn_Scores_SensorWorking", PopCornTest(
                4,
                rules.Move(64),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.Grip()
            ));
            
            AddTest(logic, "PopCorn_Filling_CheckWorking", PopCornTest(
                4 + 1,
                rules.Move(64),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.Grip(),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-30),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(-70),
                rules.Stand(0.1),
                rules.GetPopCorn()
            ));
            
            AddTest(logic, "PopCorn_Filling_Backward", PopCornTest(
                4,
                rules.Move(64),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.Grip(),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-30),
                rules.Rotate(Angle.HalfPi),
                rules.Move(70),
                rules.Stand(0.1),
                rules.GetPopCorn()
            ));
            
            AddTest(logic, "PopCorn_Filling_DispenserLimit", PopCornTest(
                4 + 5, // not 4 + 6 !
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
                rules.GetPopCorn(),
                rules.GetPopCorn(),
                rules.GetPopCorn(),
                rules.GetPopCorn(),
                rules.GetPopCorn()
            ));

            AddTest(logic, "PopCorn_Filling_CupLimit", PopCornTest(
                10,
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
                rules.GetPopCorn(),
                rules.GetPopCorn(),
                rules.GetPopCorn(),
                rules.GetPopCorn(),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-30),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.GetPopCorn(),
                rules.GetPopCorn(),
                rules.GetPopCorn(),
                rules.GetPopCorn(),
                rules.GetPopCorn()
            ));

            AddTest(logic, "PopCorn_Filling_EmptyGripper", PopCornTest(
                0,
                rules.Move(34),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(-75),
                rules.Stand(0.1),
                rules.GetPopCorn(),
                rules.GetPopCorn(),
                rules.GetPopCorn()
            ));
        }
    }
}