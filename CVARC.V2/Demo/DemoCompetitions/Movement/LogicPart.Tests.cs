using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2.SimpleMovement;
using CVARC.V2;

namespace Demo
{
    partial class MovementLogicPart
    {


        MovementTestEntry LocationTest(double X, double Y, double angleInGrad, params SimpleMovementCommand[] command)
        {
            return (client, world, asserter) =>
                {
                    var location = world.Engine.GetAbsoluteLocation(world.Actors.First().ObjectId);
                    asserter.IsEqual(0, location.X, 1e-3);
                    asserter.IsEqual(0, location.Y, 1e-3);
                    asserter.IsEqual(0, location.Yaw.Grad, 1e-3);

                    SensorsData data = null;
                    foreach(var c in command)
                        data=client.Act(c);
                    location = world.Engine.GetAbsoluteLocation(world.Actors.First().ObjectId);
                    asserter.IsEqual(X, location.X, 1e-3);
                    asserter.IsEqual(Y, location.Y, 1e-3);
                    asserter.IsEqual(angleInGrad, location.Yaw.Grad, 1e-3);
                    asserter.IsEqual(X, data.Locations[0].X, 1e-3);
                    asserter.IsEqual(Y, data.Locations[0].Y, 1e-3);
                    asserter.IsEqual(angleInGrad, data.Locations[0].Angle, 1e-3);

                };
        }


        void LoadTests()
        {
            Tests["Forward"] = new MovementTestBase(LocationTest(10, 0, 0, SimpleMovementCommand.Move(10, 1)));
            Tests["Backward"] = new MovementTestBase(LocationTest(-10, 0, 0, SimpleMovementCommand.Move(-10, 1)));
        }
    }
}
