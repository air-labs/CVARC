using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.V2.SimpleMovement;

namespace CVARC.V2.SimpleMovement
{
    public interface ISimpleMovementWorld
    {
        double LinearVelocityLimit { get; }
        Angle AngularVelocityLimit { get; }
    }

    public static class ISimpleMovementWorldExtensions
    {
        public static SimpleMovementCommand ExitCommand(this ISimpleMovementWorld world)
        {
            return new SimpleMovementCommand { WaitForExit = true };
        }

        public static SimpleMovementCommand MoveCommand(this ISimpleMovementWorld world, double distance)
        {
            return new SimpleMovementCommand
            {
                LinearVelocity = Math.Sign(distance) * world.LinearVelocityLimit,
                Duration = Math.Abs(distance / world.LinearVelocityLimit)
            };
        }

        public static SimpleMovementCommand RotateCommand(this ISimpleMovementWorld world, Angle angle)
        {
            return new SimpleMovementCommand
            {
                AngularVelocity = Math.Sign(angle.Grad) * world.AngularVelocityLimit,
                Duration = Math.Abs(angle / world.AngularVelocityLimit)
            };
        }

        public static SimpleMovementCommand ActionCommand(this ISimpleMovementWorld world, string action)
        {
            return new SimpleMovementCommand { Command = action };
        }

        public static SimpleMovementCommand SleepCommand(this ISimpleMovementWorld world, double time)
        {
            return new SimpleMovementCommand { Duration = time };
        }
    }
}