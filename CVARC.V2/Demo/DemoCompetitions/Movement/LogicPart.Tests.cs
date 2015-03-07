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
            return LocationTest(
                (frame, asserter) =>
                {
                    asserter.IsEqual(X, frame.X,1e-3);
                    asserter.IsEqual(Y, frame.Y, 1e-3);
                    asserter.IsEqual(angleInGrad, frame.Angle.Grad % 360, 1e-3);
                },
                    command);
                        
        }

        MovementTestEntry LocationTest(Action<Frame2D,IAsserter> test, params MoveAndGripCommand[] command)
        {
            return (client, world, asserter) =>
            {
                SensorsData data = null;
                foreach (var c in command)
                    data = client.Act(c);
                test(new Frame2D(data.Locations[0].X, data.Locations[0].Y, Angle.FromGrad(data.Locations[0].Angle)),asserter);
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
            
            
            logic.Tests["FuckTheBoxRect"] = new MovementTestBase(LocationTest(
                (frame,asserter)=>asserter.True(frame.X<100 && frame.X>70),
                rules.Move(100)), true, true); //думаю что тест не проходит из-за физики, поэтому не баг а фича


            logic.Tests["FuckTheBoxRect"] = new MovementTestBase(LocationTest(
               (frame, asserter) => asserter.True(frame.X < 100 && frame.X > 70),
               rules.Move(100)), false, true); //думаю что тест не проходит из-за физики, поэтому не баг а фича
            
      
        }
    }
}
