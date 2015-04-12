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
        DemoTestEntry LocationTest(double X, double Y, double angleInGrad, params MoveAndGripCommand[] command)
        {
            return LocationTest(
                (frame, asserter) =>
                {
                    asserter.IsEqual(X, frame.X, 1e-2);
                    asserter.IsEqual(Y, frame.Y, 1e-2);
                    asserter.IsEqual(angleInGrad, frame.Angle.Grad % 360, 1e-2);
                },
                    command);
        }

        DemoTestEntry LocationTest(Action<Frame2D, IAsserter> test, params MoveAndGripCommand[] command)
        {
            return (client, world, asserter) =>
            {
                DemoSensorsData data = null;
                foreach (var c in command)
                    data = client.Act(c);
                test(new Frame2D(data.Locations[0].X, data.Locations[0].Y, Angle.FromGrad(data.Locations[0].Angle)), asserter);
            };
        }


        void LoadMovementTests(LogicPart logic, MoveAndGripRules rules)
        {
            logic.Tests["Movement_Round_Forward"] = new RoundMovementTestBase(LocationTest(50, 0, 0, rules.Move(50)));
            logic.Tests["Movement_Round_Backward"] = new RoundMovementTestBase(LocationTest(-50, 0, 0, rules.Move(-50)));
            logic.Tests["Movement_Rect_Forward"] = new RectangularMovementTestBase(LocationTest(50, 0, 0, rules.Move(50)));
            logic.Tests["Movement_Rect_Backward"] = new RectangularMovementTestBase(LocationTest(-50, 0, 0, rules.Move(-50)));
            logic.Tests["Movement_Rect_Square"] = new RectangularMovementTestBase(LocationTest(0, 0, 0,
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi)));
            logic.Tests["Movement_Round_Square"] = new RoundMovementTestBase(LocationTest(0, 0, 0,
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi)));
            logic.Tests["Movement_Rect_Rotate"] = new RectangularMovementTestBase(LocationTest(0, 0, 90, rules.Rotate(Angle.HalfPi)));
            logic.Tests["Movement_Round_Rotate"] = new RoundMovementTestBase(LocationTest(0, 0, 90, rules.Rotate(Angle.HalfPi)));
            logic.Tests["Movement_Limit_Linear"] = new RoundMovementTestBase(LocationTest(50, 0, 0,
                rules.MoveWithVelocityForTime(100000, 1)));
            logic.Tests["Movement_Limit_Linear2"] = new RoundMovementTestBase(LocationTest(-50, 0, 0,
                rules.MoveWithVelocityForTime(-100000, 1)));
            logic.Tests["Movement_Limit_Round"] = new RoundMovementTestBase(LocationTest(0, 0, 0,
                rules.RotateWithVelocityForTime(Angle.Pi * 10, 4)));
            logic.Tests["Movement_Limit_Round2"] = new RoundMovementTestBase(LocationTest(0, 0, 0,
                rules.RotateWithVelocityForTime(-Angle.Pi * 19 / 10, 4)));
        }
    }
}
