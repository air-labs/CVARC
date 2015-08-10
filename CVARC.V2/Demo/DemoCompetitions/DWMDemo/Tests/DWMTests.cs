using CVARC.V2;
using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    partial class DWMLogicPartHelper
    {
        
        void LoadDWMTests(LogicPart logic, DWMRules rules)
        {
			logic.Tests["DWM_LongForward"] = new DWMDistortionTestBase(
			LocationTest(100, 0, 0, 1, rules.DWMMoveForward(100.0),
								   rules.DWMStand(1.0)));
      
            //base DWM tests
            logic.Tests["DWM_Forward"] = new DWMMovementTestBase(
                LocationTest(10, 0, 0, 1, rules.DWMMoveForward(10.0),
                                       rules.DWMStand(1.0)));
            logic.Tests["DWM_Backward"] = new DWMMovementTestBase(
                LocationTest(-10, 0, 0, 1, rules.DWMMoveForward(-10.0),
                                        rules.DWMStand(1.0)));
            logic.Tests["DWM_RightRotate"] = new DWMMovementTestBase(
                LocationTest(0, 0, 270, 1, rules.DWMRotate(AIRLab.Mathematics.Angle.FromGrad(45.0)),rules.DWMStand(1.0)));

            logic.Tests["DWM_LeftRotate"] = new DWMMovementTestBase(
                LocationTest(0, 0, 90, 1, rules.DWMRotate(-1 * AIRLab.Mathematics.Angle.FromGrad(90.0)),rules.DWMStand(1.0)));
           
            //

            logic.Tests["DWM_ArcRight"] = new DWMMovementTestBase(
                LocationTest(3, 3, 0, 1, rules.DWMMoveArc(3.0, AIRLab.Mathematics.Angle.FromGrad(90.0), true),
                                        rules.DWMStand(1.0)));

            logic.Tests["DWM_ArcLeft"] = new DWMMovementTestBase(
                LocationTest(3, -3, 0, 1, rules.DWMMoveArc(3.0, AIRLab.Mathematics.Angle.FromGrad(90.0), false),
                                        rules.DWMStand(1.0)));
            //advanced DWM tests
            logic.Tests["DWM_Turning"] = new DWMMovementTestBase(
                LocationTest(6, 6, 90, 10, rules.DWMMoveForward(3.0),
                                           rules.DWMRotate(AIRLab.Mathematics.Angle.FromGrad(90.0)),
                                           rules.DWMMoveForward(3.0),
                                           rules.DWMRotate(AIRLab.Mathematics.Angle.FromGrad(-90.0)),
                                           rules.DWMMoveForward(3.0),
                                           rules.DWMRotate(AIRLab.Mathematics.Angle.FromGrad(90.0)),
                                           rules.DWMMoveForward(3.0), rules.DWMStand(1.0)));
            logic.Tests["DWM_ArcMoving"] = new DWMMovementTestBase(
                LocationTest(6, 6, 90, 10, rules.DWMMoveArc(3.0, AIRLab.Mathematics.Angle.FromGrad(90.0), true),
                                           rules.DWMStand(1.0),
                                          rules.DWMMoveArc(3.0, AIRLab.Mathematics.Angle.FromGrad(90.0), false)));

            //вперед, назад, повороты
            //езда по дугам 
            //пара-тройка составных тестов, например скругленный прямоугольник

        }
    }
}
