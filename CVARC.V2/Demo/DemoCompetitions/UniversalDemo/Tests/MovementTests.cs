using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;

namespace Demo
{
    partial class DemoLogicPartHelper
    {
        DemoTestEntry LocationTest(double X, double Y, double angleInGrad, double tolerance, params DemoCommand[] command)
        {
            return LocationTest(
                (frame, asserter) =>
                {
                    asserter.IsEqual(X, frame.X,1e-2*tolerance);
                    asserter.IsEqual(Y, frame.Y, 1e-2 * tolerance);
                    asserter.IsEqual(angleInGrad, frame.Angle.Grad % 360, 1e-1 * tolerance);
                },
                    command);
        }

        DemoTestEntry LocationTest(Action<Frame2D, IAsserter> test, params DemoCommand[] command)
        {
            return (client, world, asserter) =>
            {
                DemoSensorsData data = null;
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


        void LoadMovementTests(LogicPart logic, DemoRules rules)
        {
            logic.Tests["Movement_Round_Forward"] = new RoundMovementTestBase(LocationTest(10,0, 0, 1, rules.Move(10)));
			logic.Tests["Movement_Round_Backward"] = new RoundMovementTestBase(LocationTest(-10, 0, 0, 1, rules.Move(-10)));
            logic.Tests["Movement_Rect_Forward"] = new RectangularMovementTestBase(LocationTest(10, 0, 0, 1, rules.Move(10)));
			logic.Tests["Movement_Rect_Backward"] = new RectangularMovementTestBase(LocationTest(-10, 0, 0, 1, rules.Move(-10)));
            logic.Tests["Movement_Rect_Square"] = new RectangularMovementTestBase(LocationTest(0, 0, 0, 50,
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi)));
			logic.Tests["Movement_Round_Square"] = new RoundMovementTestBase(LocationTest(0, 0, 0, 10,
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi)));
			logic.Tests["Movement_Rect_Rotate"] = new RectangularMovementTestBase(LocationTest(0, 0, 90, 10, rules.Rotate(Angle.HalfPi)));
            logic.Tests["Movement_Round_Rotate"] = new RoundMovementTestBase(LocationTest(0, 0, 90, 10, rules.Rotate(Angle.HalfPi)));
          
            logic.Tests["Movement_Limit_Linear"] = new RoundMovementTestBase(LocationTest(50, 0, 0, 1,
                rules.MoveWithVelocityForTime(100000, 1)));
			logic.Tests["Movement_Limit_Round"] = new RoundMovementTestBase(LocationTest(0, 0, 0, 1,
                rules.MoveWithVelocityForTime(-100000, 1)));
            logic.Tests["Movement_Limit_Round"] = new RoundMovementTestBase(LocationTest(0, 0, 0, 1,
                rules.RotateWithVelocityForTime(Angle.Pi * 10, 4)));
            logic.Tests["Movement_Limit_Round2"] = new RoundMovementTestBase(LocationTest(0, 0, 0, 1,
                rules.RotateWithVelocityForTime(-Angle.Pi * 19 / 10, 4)));
        }
    }
}
