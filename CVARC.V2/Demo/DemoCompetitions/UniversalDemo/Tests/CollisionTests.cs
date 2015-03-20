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
        DemoTestEntry CollisionTest(int count, int left, int right, params MoveAndGripCommand[] command)
        {
            return (client, world, asserter) =>
            {
                DemoSensorsData result = null;
                foreach (var c in command)
                    result = client.Act(c);

				
                asserter.IsEqual(count, result.Collisions.Where(z => z.Guilty).Count(), 0);
				asserter.IsEqual(left, result.Collisions.Where(z => z.Guilty && z.CollisionCase.Offender.ControllerId == TwoPlayersId.Left).Count(), 0);
				asserter.IsEqual(right, result.Collisions.Where(z => z.Guilty && z.CollisionCase.Offender.ControllerId == TwoPlayersId.Right).Count(), 0);
			};
        }

        private void LoadCollisionTests(LogicPart logic, MoveAndGripRules rules)
        {
            logic.Tests["Collision_Rect_NoCollision"] = new RectangularCollisionTestBase(CollisionTest(0, 0, 0,
                rules.Grip(),

                rules.MoveWithVelocityForTime(-50, 0.2),
                rules.MoveWithVelocityForTime(50, 0.2)));
            logic.Tests["Collision_Rect_CollisionCount"] = new RectangularCollisionTestBase(CollisionTest(3, 3, 0,
                rules.MoveWithVelocityForTime(50, 1),
                rules.MoveWithVelocityForTime(50, 0.8),
                rules.MoveWithVelocityForTime(50, 0.3),
                rules.MoveWithVelocityForTime(-50, 0.2),
                rules.MoveWithVelocityForTime(50, 0.3),
                rules.MoveWithVelocityForTime(-50, 0.2),
                rules.MoveWithVelocityForTime(50, 0.3),
                rules.MoveWithVelocityForTime(-50, 0.2)));
            logic.Tests["Collision_Rect_CollisionBox"] = new RectangularCollisionTestBase(LocationTest(1, 1, 0,
                rules.Grip(),
                rules.MoveWithVelocityForTime(50, 1.5),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
                rules.MoveWithVelocityForTime(50, 0.45)));
            logic.Tests["Collision_Rect_NoBox"] = new RectangularCollisionTestBase(LocationTest(3, 3, 0,
                rules.MoveWithVelocityForTime(50, 1.05),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
                rules.MoveWithVelocityForTime(50, 0.8),
                rules.MoveWithVelocityForTime(50, 0.3),
                rules.MoveWithVelocityForTime(-50, 0.2),
                rules.MoveWithVelocityForTime(50, 0.3),
                rules.MoveWithVelocityForTime(-50, 0.2),
                rules.MoveWithVelocityForTime(50, 0.3),
                rules.MoveWithVelocityForTime(-50, 0.2)));
            logic.Tests["Collision_Rect_RotateNoBox"] = new RectangularCollisionTestBase(LocationTest(1, 1, 0,
                rules.MoveWithVelocityForTime(50, 1.7),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
                rules.MoveWithVelocityForTime(50, 1.45),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
                rules.MoveWithVelocityForTime(50, 0.7),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1)));
            logic.Tests["Collision_Rect_RotateBox"] = new RectangularCollisionTestBase(LocationTest(1, 1, 0,
                rules.Grip(),
                rules.MoveWithVelocityForTime(50, 1.7),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
                rules.MoveWithVelocityForTime(50, 1.6),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
                rules.MoveWithVelocityForTime(50, 0.7),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1)));
        }
    }
}		