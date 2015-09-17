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
        DWMTestEntry LocationTest(double X, double Y, double angleInGrad, double tolerance, params DWMCommand[] command)
        {
            return LocationTest(
                (frame, asserter) =>
                {
                    asserter.IsEqual(X, frame.X, 1e-2 * tolerance);
                    asserter.IsEqual(Y, frame.Y, 1e-2 * tolerance);
                    asserter.IsEqual(angleInGrad, frame.Angle.Grad % 360, 1e-1 * tolerance);
                },
                    command);

        }

        DWMTestEntry LocationTest(Action<Frame2D, IAsserter> test, params DWMCommand[] command)
        {
            return (client, world, asserter) =>
            {
                DWMSensorsData data = null;
                int x = 0;
                foreach (var c in command)
                {
                    Debugger.Log(DebuggerMessageType.UnityTest, "Before performance");
                    data = client.Act(c);
                    Debugger.Log(DebuggerMessageType.UnityTest, String.Format("Performed: {0} {1}", c.ToString(), x++));
                }
                Debugger.Log(DebuggerMessageType.UnityTest, "All commands performed");
                test(new Frame2D(data.Locations[0].X, data.Locations[0].Y, Angle.FromGrad(data.Locations[0].Angle)), asserter);
            };
        }

        void LoadDWMTests(LogicPart logic, DWMRules rules)
        {
            logic.Tests["DWM_Movement_LongForward"] = new DWMMovementTestBase(
			LocationTest(100, 0, 0, 10, rules.DWMMoveForward(100.0),
								   rules.DWMStand(1.0)));
            //basic DWM tests
            logic.Tests["DWM_Movement_Forward"] = new DWMMovementTestBase(
                LocationTest(10, 0, 0, 10, rules.DWMMoveForward(10.0),
                                       rules.DWMStand(1.0)));
            logic.Tests["DWM_Movement_Backward"] = new DWMMovementTestBase(
                LocationTest(-10, 0, 0, 10, rules.DWMMoveForward(-10.0),
                                        rules.DWMStand(1.0)));

            logic.Tests["DWM_Movement_RightRotate"] = new DWMMovementTestBase(
                LocationTest(0, 0, -45, 10, rules.DWMRotate(AIRLab.Mathematics.Angle.FromGrad(45.0)),rules.DWMStand(1.0)));

            logic.Tests["DWM_Movement_LeftRotate"] = new DWMMovementTestBase(
                LocationTest(0, 0, 45, 10, rules.DWMRotate(-1 * AIRLab.Mathematics.Angle.FromGrad(45.0)),rules.DWMStand(1.0)));

            logic.Tests["DWM_Movement_ArcRight"] = new DWMMovementTestBase(
                LocationTest(3, 3, 0, 10, rules.DWMMoveArc(3.0, AIRLab.Mathematics.Angle.FromGrad(90.0), true),
                                        rules.DWMStand(1.0)));

            logic.Tests["DWM_Movement_ArcLeft"] = new DWMMovementTestBase(
                LocationTest(3, -3, 0, 10, rules.DWMMoveArc(3.0, AIRLab.Mathematics.Angle.FromGrad(90.0), false),
                                        rules.DWMStand(1.0)));
            //advanced DWM tests
            logic.Tests["DWM_Movement_Turning"] = new DWMMovementTestBase(
                LocationTest(6, 6, 90, 10, rules.DWMMoveForward(3.0),
                                           rules.DWMRotate(AIRLab.Mathematics.Angle.FromGrad(90.0)),
                                           rules.DWMMoveForward(3.0),
                                           rules.DWMRotate(AIRLab.Mathematics.Angle.FromGrad(-90.0)),
                                           rules.DWMMoveForward(3.0),
                                           rules.DWMRotate(AIRLab.Mathematics.Angle.FromGrad(90.0)),
                                           rules.DWMMoveForward(3.0), rules.DWMStand(1.0)));

            logic.Tests["DWM_Movement_ArcMoving"] = new DWMMovementTestBase(
                LocationTest(6, 6, 90, 10, rules.DWMMoveArc(3.0, AIRLab.Mathematics.Angle.FromGrad(90.0), true),
                                           rules.DWMStand(1.0),
                                          rules.DWMMoveArc(3.0, AIRLab.Mathematics.Angle.FromGrad(90.0), false)));

        }
    }
}
