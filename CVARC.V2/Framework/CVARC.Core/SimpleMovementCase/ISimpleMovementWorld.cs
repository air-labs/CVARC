using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.V2.SimpleMovement;

namespace CVARC.V2.SimpleMovement
{

    public class SimpleMovementCommandHelper
    {
        public double LinearVelocityLimit { get; set; }
        public Angle AngularVelocityLimit { get; set; }
        public SimpleMovementCommand ExitCommand()
        {
            return new SimpleMovementCommand { WaitForExit = true };
        }

        public SimpleMovementCommand Move(double distance)
        {
            return new SimpleMovementCommand
            {
                LinearVelocity = Math.Sign(distance) * LinearVelocityLimit,
                Duration = Math.Abs(distance / LinearVelocityLimit)
            };
        }

        public SimpleMovementCommand Rotate(Angle angle)
        {
            return new SimpleMovementCommand
            {
                AngularVelocity = Math.Sign(angle.Grad) * AngularVelocityLimit,
                Duration = Math.Abs(angle / AngularVelocityLimit)
            };
        }

        public SimpleMovementCommand ActionCommand(string action)
        {
            return new SimpleMovementCommand { Command = action };
        }

        public SimpleMovementCommand SleepCommand(double time)
        {
            return new SimpleMovementCommand { Duration = time };
        }
    }

    public interface ISimpleMovementWorld
    {
        SimpleMovementCommandHelper CommandHelper { get; }
    }

}