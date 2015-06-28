using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;

namespace Demo
{
    partial class DemoLogicPartHelper
    {
        DemoTestEntry CollisionTest(int count, int left, int right, params DemoCommand[] command)
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

		DemoTestEntry ComplexCollisionTest(int count, double time, bool first, bool second, params DemoCommand[] command)
        {
            return (client, world, asserter) =>
            {
                DemoSensorsData result = null;
                foreach (var c in command)
                    result = client.Act(c);
                asserter.IsEqual(count, result.Collisions.Count, 0);
                asserter.IsEqual(time, result.Collisions[count-2].Time, 0.2);
                asserter.IsEqual(first, result.Collisions[count-2].Guilty);
                asserter.IsEqual(second, result.Collisions[count-1].Guilty);
                asserter.IsEqual(result.Collisions[count-2].Time, result.Collisions[count-1].Time, 0);
            };
        }
		DemoTestEntry CollisionTest(int count, bool first, bool second, params DemoCommand[] command)
        {
            return (client, world, asserter) =>
            {
                DemoSensorsData result = null;
                foreach (var c in command)
                    result = client.Act(c);
                asserter.IsEqual(count, result.Collisions.Count, 0);
                asserter.IsEqual(first, result.Collisions[count - 2].Guilty);
                asserter.IsEqual(second, result.Collisions[count - 1].Guilty);
                asserter.IsEqual(result.Collisions[count - 2].Time, result.Collisions[count - 1].Time, 0);
            };
        }

        private void LoadCollisionTests(LogicPart logic, DemoRules rules)
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
            logic.Tests["Collision_Rect_CollisionBox"] = new RectangularCollisionTestBase(CollisionTest(1, 1, 0,
                rules.Grip(),
                rules.MoveWithVelocityForTime(50, 1.5),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
                rules.MoveWithVelocityForTime(50, 0.45)));
            logic.Tests["Collision_Rect_NoBox"] = new RectangularCollisionTestBase(CollisionTest(3, 3, 0,
                rules.MoveWithVelocityForTime(50, 1.05),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
                rules.MoveWithVelocityForTime(50, 0.8),
                rules.MoveWithVelocityForTime(50, 0.3),
                rules.MoveWithVelocityForTime(-50, 0.2),
                rules.MoveWithVelocityForTime(50, 0.3),
                rules.MoveWithVelocityForTime(-50, 0.2),
                rules.MoveWithVelocityForTime(50, 0.3),
                rules.MoveWithVelocityForTime(-50, 0.2)));
            logic.Tests["Collision_Rect_RotateNoBox"] = new RectangularCollisionTestBase(CollisionTest(1, 1, 0,
                rules.MoveWithVelocityForTime(50, 1.7),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
                rules.MoveWithVelocityForTime(50, 1.45),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
                rules.MoveWithVelocityForTime(50, 0.7),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1)));
            logic.Tests["Collision_Rect_RotateBox"] = new RectangularCollisionTestBase(CollisionTest(1, 1, 0,
                rules.Grip(),
                rules.MoveWithVelocityForTime(50, 1.7),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
                rules.MoveWithVelocityForTime(50, 1.6),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
                rules.MoveWithVelocityForTime(50, 0.7),
                rules.RotateWithVelocityForTime(Angle.HalfPi, 1)));
            logic.Tests["Collision_Rect_WallCollisionTime"] = new RectangularCollisionTestBase(ComplexCollisionTest(
                2,
                2.26,//вероятно после изменения размера робота эта велечина должна была измениться до 2,5-2,6
                false,
                false,
                rules.Rotate(Angle.HalfPi),
                rules.Move(50),
                rules.Move(50)
                ));
            logic.Tests["Collision_Rect_PushingWithBrick"] = new RectangularCollisionTestBase(ComplexCollisionTest(
                2,
                8.5,
                true,
                false,
                rules.Rotate(Angle.Pi),
                rules.Move(30),
                rules.Stand(0.00001),
                rules.Grip(),
                rules.Move(-30),
                rules.Rotate(Angle.HalfPi),
                rules.Move(25),
                rules.Rotate(Angle.HalfPi),
                rules.Move(40),
                rules.Rotate(Angle.HalfPi)
                ));
            logic.Tests["Collision_Rect_RotateWallCollision"] = new RectangularCollisionTestBase2(ComplexCollisionTest(
                2,
                8.5, //при удалении костыля время надо тоже уменьшать на 2 сек
                false,
                false,
                rules.Rotate(Angle.Pi),
                rules.Move(30),
                rules.Stand(0.00001),
                rules.Grip(),
                rules.Rotate(-Angle.HalfPi),
                //rules.Stand(2), //костыль, чтоб тест прошел
                rules.Move(70),
                rules.Rotate(-Angle.Pi)
                ));
            logic.Tests["Collision_Rect_BrickCollision"] = new RectangularCollisionTestBase2(
                CollisionTest(
                    4,
                    true,
                    false,
                    rules.Rotate(Angle.HalfPi),
                    rules.Move(50),
                    rules.Rotate(-Angle.HalfPi),
                    rules.Move(45),
                    rules.Rotate(-Angle.HalfPi),
                    rules.Move(70)
                ));
            logic.Tests["Collision_Rect_WallAndBrickCollision"] = new RectangularCollisionTestBase2(
                CollisionTest(
                    4,
                    false,
                    false,
                    rules.Rotate(Angle.HalfPi),
                    rules.Move(15),
                    rules.Rotate(-Angle.HalfPi),
                    rules.Move(45),
                    rules.Rotate(Angle.HalfPi),
                    rules.Move(70)
                ));

        }
    }
}
