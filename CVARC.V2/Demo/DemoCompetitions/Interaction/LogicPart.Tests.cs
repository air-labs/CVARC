using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2.SimpleMovement;
using CVARC.V2;

namespace Demo
{
    partial class InteractionLogicPart
    {


        InteractionTestEntry InteractionTest(bool flag, params SimpleMovementCommand[] command)
        {
            return (client, world, asserter) =>
            {
                InteractionSensorData data = null;
                foreach (var c in command)
                    data = client.Act(c);
                //asserter.IsEqual(false, data != null);
                asserter.IsEqual(flag, data.IsGripped);
            };
        }


        void LoadTests()
        {

            Tests["Grip"] = new InteractionTestBase(InteractionTest(true,
                SimpleMovementCommand.Rotate(-Angle.HalfPi,1),
                SimpleMovementCommand.Move(15,1),
                SimpleMovementCommand.Action("Grip"),
                SimpleMovementCommand.Move(-10,1)), true);
            Tests["GripThroughWall"] = new InteractionTestBase(InteractionTest(false,
                SimpleMovementCommand.Rotate(Angle.HalfPi,1),
                SimpleMovementCommand.Move(50,1),
                SimpleMovementCommand.Action("Grip"),
                SimpleMovementCommand.Move(-50,1)), true);
            Tests["Release"] = new InteractionTestBase(InteractionTest(false,
                SimpleMovementCommand.Rotate(Angle.Pi,2),
                SimpleMovementCommand.Move(15,1),
                SimpleMovementCommand.Action("Grip"),
                SimpleMovementCommand.Move(-50,1),
                SimpleMovementCommand.Action("Release"),
                SimpleMovementCommand.Rotate(Angle.Pi, 1)), true);
            Tests["GripUnGripable"] = new InteractionTestBase(InteractionTest(false,
                SimpleMovementCommand.Move(25,1),
                SimpleMovementCommand.Rotate(Angle.HalfPi,1),
                SimpleMovementCommand.Move(25, 1),
                SimpleMovementCommand.Action("Grip"),
                SimpleMovementCommand.Move(-25, 1)), true);
        }
    }
}
