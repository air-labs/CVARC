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
        public void LoadTowerBuilderTests(LogicPart logic, RMRules rules)
        {
            AddTest(logic, "Tower_Grip_SingleDetail", TowerBuilderTest(
                1,
                rules.Move(65),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect()
            ));

            AddTest(logic, "Tower_Grip_MultipleDetails", TowerBuilderTest(
                2,
                rules.Move(65),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(15),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Stand(1),
                rules.Collect()
            ));

            AddTest(logic, "Tower_Grip_LightBeforeStand", TowerBuilderTest(
                1,
                rules.Rotate(Angle.Pi),
                rules.Move(10),
                rules.Stand(0.1),
                rules.Collect()
            ));

            AddTest(logic, "Tower_Grip_LightAfterStand", TowerBuilderTest(
                1,
                rules.Move(65),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Rotate(Angle.Pi),
                rules.Move(25),
                rules.Rotate(Angle.HalfPi),
                rules.Move(75),
                rules.Stand(0.1),
                rules.Collect()
            ));
            
            AddTest(logic, "Tower_Release_SingleDetail", TowerBuilderTest(
                0,
                rules.Move(65),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.BuildTower()
            ));

            AddTest(logic, "Tower_Release_MultipleDetail", TowerBuilderTest(
                0,
                rules.Move(65),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(15),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Stand(0.1),
                rules.Collect(),
                rules.BuildTower()
            ));

            AddTest(logic, "Tower_Grip_AlreadyConnected", TowerBuilderTest(
                2,
                rules.Move(65),
                rules.Rotate(-Angle.HalfPi),
                rules.Move(25),
                rules.Stand(0.1),
                rules.Collect(),
                rules.Move(15),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Stand(0.1),
                rules.Collect(),
                rules.BuildTower(),
                rules.Collect()
            ));
        }
    }
}
