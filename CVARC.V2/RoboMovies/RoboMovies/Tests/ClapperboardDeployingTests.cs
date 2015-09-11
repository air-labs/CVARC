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
        public void LoadClapperboardDeployingTests(LogicPart logic, RMRules rules)
        {
            AddTest(logic, "Clapper_Scores_RightDeployer", ScoreTest(5,
                rules.Move(55),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(85),
                rules.Rotate(Angle.HalfPi),
                rules.Stand(0.1),
                rules.UseRightDeployer()
            ));
            
            AddTest(logic, "Clapper_Scores_LeftDeployer", ScoreTest(5,
                rules.Move(55),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(85),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.UseLeftDeployer()
            ));

            AddTest(logic, "Clapper_Scores_RemoteDeployingShouldBeFailure", ScoreTest(0,
                rules.Move(55),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(65),
                rules.Rotate(-Angle.HalfPi),
                rules.Stand(0.1),
                rules.UseLeftDeployer()
            ));

            AddTest(logic, "Clapper_Scores_BackDeployingNotEnough", ScoreTest(0,
                rules.Move(50),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-85),
                rules.Stand(0.1),
                rules.UseLeftDeployer(),
                rules.UseRightDeployer()
            ));

            AddTest(logic, "Clapper_Scores_WrongDeployingObjects", ScoreTest(0,
                rules.Move(200),
                rules.Rotate(Angle.HalfPi),
                rules.Stand(0.1),
                rules.UseRightDeployer(),
                rules.Stand(0.1),
                rules.Rotate(Angle.HalfPi),
                rules.Move(125),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Rotate(Angle.HalfPi),
                rules.Stand(0.1),
                rules.UseRightDeployer()
            ));
            
            AddTest(logic, "Clapper_Scores_WrongClapperboardColor", ScoreTest(-10,
                rules.Move(25),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(85),
                rules.Rotate(Angle.HalfPi),
                rules.Stand(0.1),
                rules.UseRightDeployer()
            ));


            /* не работает, нужно поменять константы
             *
             * AddTest(logic, "Clapper_Scores_AllClapperboardsClose", ScoreTest(30,
             *     rules.Move(50),
             *     rules.Rotate(Angle.HalfPi),
             *     rules.Move(-90),
             *     rules.Rotate(Angle.HalfPi),
             *     rules.Move(45),
             *     rules.Stand(0.1),
             *     rules.UseLeftDeployer(),
             *     rules.Stand(0.1),
             *     rules.Move(-25),
             *     rules.Stand(0.1),
             *     rules.UseLeftDeployer(),
             *     rules.Stand(0.1),
             *     rules.Move(-25),
             *     rules.Stand(0.1),
             *     rules.UseLeftDeployer(),
             *     rules.Stand(0.1),
             *     rules.Rotate(-Angle.HalfPi),
             *     rules.Move(40),
             *     rules.Rotate(-Angle.HalfPi),
             *     rules.Move(150),
             *     rules.Rotate(-Angle.HalfPi),
             *     rules.Move(40),
             *     rules.Rotate(Angle.HalfPi),
             *     rules.Stand(0.1),
             *     rules.UseRightDeployer(),
             *     rules.Move(-25),
             *     rules.Stand(0.1),
             *     rules.UseRightDeployer(),
             *     rules.Move(-25),
             *     rules.Stand(0.1),
             *     rules.UseRightDeployer()
             * ));
             */
        }
    }
}
