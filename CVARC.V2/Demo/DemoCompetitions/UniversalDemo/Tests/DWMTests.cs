using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    partial class DemoLogicPartHelper
    {
        void LoadDWMTests(LogicPart logic, DemoRules rules)
        {
            logic.Tests["DWM_Forward"] = new RoundMovementTestBase(
                LocationTest(10, 0, 0, rules.DWMMoveForward(10.0),
                                       rules.DWMStand(1.0)));
            logic.Tests["DWM_Backward"] = new RoundMovementTestBase(
                LocationTest(-10, 0, 0, rules.DWMMoveForward(-10.0),
                                        rules.DWMStand(1.0)));
            logic.Tests["DWM_ForwardAndReturn"] = new RoundMovementTestBase(
                LocationTest(0, 0, 0, rules.DWMMoveForward(10.0),
                                      rules.DWMStand(1.0),
                                      rules.DWMMoveForward(-10.0),
                                      rules.DWMStand(1.0)));
            logic.Tests["DWM_RotateAndReturn"] = new RoundMovementTestBase(
                LocationTest(0, 0, 0, rules.DWMRotate(AIRLab.Mathematics.Angle.HalfPi),
                                      rules.DWMStand(1.0),
                                      rules.DWMRotate(-1 * AIRLab.Mathematics.Angle.HalfPi),
                                      rules.DWMStand(1.0)));
            logic.Tests["DWM_SquareMoving"] = new RoundMovementTestBase(
                LocationTest(0, 0, 0, rules.DWMMoveForward(5.0),
                                      rules.DWMRotate(AIRLab.Mathematics.Angle.HalfPi), 
                                      rules.DWMMoveForward(5.0),
                                      rules.DWMRotate(AIRLab.Mathematics.Angle.HalfPi),
                                      rules.DWMMoveForward(5.0),
                                      rules.DWMRotate(AIRLab.Mathematics.Angle.HalfPi),
                                      rules.DWMMoveForward(5.0), rules.DWMStand(0.5)
                                      ));
            //вперед, назад, повороты
            //езда по дугам 
            //пара-тройка составных тестов, например скругленный прямоугольник

        }
    }
}
