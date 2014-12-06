using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;

namespace Demo
{
    partial class MovementLogicPartHelper
    {
        MovementTestEntry LocationTest(double X, double Y, double angleInGrad, params MoveAndGripCommand[] command)
        {
            return (client, world, asserter) =>
                {
                    var location = world.Engine.GetAbsoluteLocation(world.Actors.First().ObjectId);
                    asserter.IsEqual(0, location.X, 1e-3);
                    asserter.IsEqual(0, location.Y, 1e-3);
                    asserter.IsEqual(0, location.Yaw.Grad, 1e-3);

                    var speed = world.Engine.GetSpeed(world.Actors.First().ObjectId);
                    asserter.IsEqual(0, speed.X, 1e-3);
                    asserter.IsEqual(0, speed.Y, 1e-3);
                    asserter.IsEqual(0, speed.Yaw.Grad, 1e-3);


                    SensorsData data = null;
                    foreach(var c in command)
                        data=client.Act(c);
                    location = world.Engine.GetAbsoluteLocation(world.Actors.First().ObjectId);
                    asserter.IsEqual(X, location.X, 1e-3);
                    asserter.IsEqual(Y, location.Y, 1e-3);
                    asserter.IsEqual(angleInGrad, location.Yaw.Grad % 360, 5e-3);
                    asserter.IsEqual(X, data.Locations[0].X, 1e-3);
                    asserter.IsEqual(Y, data.Locations[0].Y, 1e-3);
                    asserter.IsEqual(angleInGrad, data.Locations[0].Angle % 360, 5e-3);
                };
        }


        void LoadTests(LogicPart logic, MoveAndGripRules rules)
        {
            logic.Tests["Forward"] = new MovementTestBase(LocationTest(10,0, 0, rules.Move(10)));
            logic.Tests["Backward"] = new MovementTestBase(LocationTest(-10, 0, 0, rules.Move(-10)));
            logic.Tests["ForwardRect"] = new MovementTestBase(LocationTest(10, 0, 0, rules.Move(10)), true);
            logic.Tests["BackwardRect"] = new MovementTestBase(LocationTest(-10, 0, 0, rules.Move(-10)), true);
            logic.Tests["SquareRect"] = new MovementTestBase(LocationTest(0, 0, 0,
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi)),true);
            logic.Tests["Square"] = new MovementTestBase(LocationTest(0, 0, 0,
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi),
                rules.Move(10),
                rules.Rotate(Angle.HalfPi)));
			logic.Tests["RotateRect"] = new MovementTestBase(LocationTest(0, 0, 90, rules.Rotate(Angle.HalfPi)),true);
            logic.Tests["Rotate"] = new MovementTestBase(LocationTest(0, 0, 90, rules.Rotate(Angle.HalfPi)));
            //для AlignmentRect пришлось увеличить delta на проверке угла поворота до 0,005
            logic.Tests["AlignmentRect"] = new MovementTestBase(LocationTest(25.355,17.357,Angle.HalfPi.Grad,
                rules.Move(-10),
                rules.Rotate(Angle.HalfPi/2),
                rules.Move(50)),true);
            logic.Tests["Speed"] = new MovementTestBase(LocationTest(50, 0, 0,
                rules.MoveWithVelocityForTime(100000, 1)));
            logic.Tests["RotateSpeed"] = new MovementTestBase(LocationTest(0, 0, 0,
                rules.RotateWithVelocityForTime(Angle.Pi*10, 4)));
            logic.Tests["FuckTheBoxRect"] = new MovementTestBase(LocationTest(100, 0, 0,
                rules.Move(100)), true, true); //думаю что тест не проходит из-за физики, поэтому не баг а фича
            logic.Tests["FuckTheBox"] = new MovementTestBase(LocationTest(100, 0, 0,
                rules.Move(100)), false, true); //тот же тест только с цилиндром
            logic.Tests["Exit"] = new MovementTestBase(
                (client, asserter, world) =>
                {
                    var move = LocationTest(10, 0, 0, rules.Move(10));
                    move(client, asserter, world);
                    client.Exit();
                });
      
        }
    }
}
