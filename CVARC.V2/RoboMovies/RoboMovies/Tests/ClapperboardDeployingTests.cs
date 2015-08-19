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
            AddTest(logic, "Clapper_Scores_UsualDeploy", ScoreTest(10,
                rules.Move(55),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(85),
                rules.Rotate(Angle.HalfPi),
                rules.Stand(0.1),
                rules.UseRightDeployer(),
                rules.Stand(0.1)
            ));
            
            AddTest(logic, "Clapper_Scores_BackDeployingNotEnough", ScoreTest(0,
                rules.Move(50),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-120),
                rules.Stand(0.1),
                rules.UseLeftDeployer(),
                rules.Stand(0.1)
                ));


            //тайм лимит:
            //при TL = 20 он почему-то завершает тест через 10 секунд, хотя CombinedUnit по идее должен возвращать 1 секунду за действие хлопушкой
            //при TL = 30 тест завершается успешно
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
            AddTest(logic, "Clapper_Scores_WrongClapperboardColor", ScoreTest(0,
                rules.Move(50),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-90),
                rules.Rotate(Angle.HalfPi),
                rules.Move(15),
                rules.Stand(0.1),
                rules.UseLeftDeployer(),
                rules.Stand(0.1),
                rules.Rotate(Angle.Pi),
                rules.Stand(0.1)
                ));
            AddTest(logic, "Clapper_Scores_AllClapperboardsClose", ScoreTest(30,
                rules.Move(50),
                rules.Rotate(Angle.HalfPi),
                rules.Move(-90),
                rules.Rotate(Angle.HalfPi),
                rules.Move(45),
                rules.Stand(0.1),
                rules.UseLeftDeployer(),
                rules.Stand(0.1),
                rules.Move(-25),
                rules.Stand(0.1),
                rules.UseLeftDeployer(),
                rules.Stand(0.1),
                rules.Move(-25),
                rules.Stand(0.1),
                rules.UseLeftDeployer(),
                rules.Stand(0.1),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(40),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(150),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(40),
                rules.Rotate(Angle.HalfPi),
                rules.Stand(0.1),
                rules.UseRightDeployer(),
                rules.Move(-25),
                rules.Stand(0.1),
                rules.UseRightDeployer(),
                rules.Move(-25),
                rules.Stand(0.1),
                rules.UseRightDeployer()
                ));
        }
    }
}
