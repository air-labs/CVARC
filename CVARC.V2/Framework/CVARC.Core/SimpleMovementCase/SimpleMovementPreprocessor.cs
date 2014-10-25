using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;

namespace CVARC.V2.SimpleMovement
{
    public class SimpleMovementPreprocessor : CommandPreprocessor<SimpleMovementCommand,ISimpleMovementRobot>
    {


        public override IEnumerable<SimpleMovementCommand> Preprocess(object cmd)
        {
            if (!(cmd is SimpleMovementCommand))
                throw new Exception("SimpleMovementCommand is expected, but the command was " + cmd.GetType().Name);
            var command = cmd as SimpleMovementCommand;

            double rightDuration;
            if (command.Command != null)
            {
                yield return new SimpleMovementCommand { LinearVelocity = 0, AngularVelocity = Angle.Zero, Duration = 0 };
                yield return new SimpleMovementCommand { Command = command.Command, Duration = GetDurationForCustomCommand(command) };
                yield break;
            }

            if (command.WaitForExit)
            {
                yield return new SimpleMovementCommand { LinearVelocity = 0, AngularVelocity = Angle.Zero, Duration = double.PositiveInfinity };
                yield break;
            }
            var World = Actor.World;

            var linear = command.LinearVelocity;
            if (Math.Abs(linear) > World.CommandHelper.LinearVelocityLimit)
                linear = Math.Sign(linear) * World.CommandHelper.LinearVelocityLimit;

            var angular = command.AngularVelocity;
            if (Math.Abs(angular.Grad) > World.CommandHelper.AngularVelocityLimit.Grad)
                angular = Angle.FromGrad(Math.Sign(angular.Grad) * World.CommandHelper.AngularVelocityLimit.Grad);

            if (linear != 0) angular = Angle.Zero;
            yield return new SimpleMovementCommand { LinearVelocity = linear, AngularVelocity = angular, Duration = command.Duration };
        }

        public virtual int GetDurationForCustomCommand(SimpleMovementCommand command)
        {
            return 1;
        }
    }
}
