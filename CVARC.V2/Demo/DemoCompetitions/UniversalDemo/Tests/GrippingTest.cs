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
        DemoTestEntry GrippingTest(bool flag, params MoveAndGripCommand[] command)
        {
            return (client, world, asserter) =>
            {
                DemoSensorsData data = new DemoSensorsData();
                foreach (var c in command)
                    data = client.Act(c);
                asserter.IsEqual(flag, data.IsGripped);
            };
        }

        void LoadGrippingTests(LogicPart logic, MoveAndGripRules rules)
        {
            logic.Tests["Gripping_Rect_Grip"] = new RectangularGrippingTestBase(GrippingTest(
                true,
                rules.Move(50),
                rules.Stand(1),
                rules.Grip(),
                rules.Move(-10),
                rules.Stand(5)
                ));

            logic.Tests["Gripping_Rect_GripThroughWall"] = new RectangularGrippingTestBase(GrippingTest(
                false,
                rules.Rotate(Angle.HalfPi),
                rules.Move(50),
                rules.Grip(),
                rules.Move(-50)));
            logic.Tests["Gripping_Rect_Release"] = new RectangularGrippingTestBase(GrippingTest(
                false,
                rules.Move(15),
                rules.Grip(),
                rules.Move(-50),
                rules.Release(),
                rules.Rotate(Angle.Pi)));
            logic.Tests["Gripping_Rect_GripUnGripable"] = new RectangularGrippingTestBase(GrippingTest(
                false,
                rules.Move(25),
                rules.Rotate(Angle.HalfPi),
                rules.Move(25),
                rules.Grip(),
                rules.Move(-25)));
            logic.Tests["Gripping_Rect_GripFromBack"] = new RectangularGrippingTestBase(
                GrippingTest(
                    false,
                    rules.Move(-40),
                    rules.Stand(1),
                    rules.Grip()));
        }
    }
}
