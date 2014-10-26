using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2.SimpleMovement;
namespace Demo
{
    public class MultiCommandPreprocessor : CommandPreprocessor<SimpleMovementCommand,IActor>
    {

        public override IEnumerable<SimpleMovementCommand> Preprocess(object command)
        {
            if (command is SimpleMovementCommand)
            {
                yield return command as SimpleMovementCommand;
                yield break;
            }
            if (!(command is Displacement))
                throw new Exception("Unexpected command type " + command.GetType());

            var angularVelocity = Angle.FromGrad(90);
            var d = command as Displacement;
            var desiredAngle = Angle.FromRad(Math.Atan2(d.DY, d.DX));
            var location = Actor.World.Engine.GetAbsoluteLocation(Actor.ObjectId);
            if (Math.Abs(location.Yaw.Grad - desiredAngle.Grad) > 0.1)
            {
                var delta = (desiredAngle - location.Yaw).Simplify180();
                yield return SimpleMovementCommand.RotateWithVelocity(delta, angularVelocity);
            }
            if (d.DX != 0)
                yield return SimpleMovementCommand.MoveWithVelocity(Math.Abs(d.DX), 50);
            else
                yield return SimpleMovementCommand.MoveWithVelocity(Math.Abs(d.DY), 50);
        }
    }
}
