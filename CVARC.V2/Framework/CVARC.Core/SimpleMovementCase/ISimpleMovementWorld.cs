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
        public SimpleMovementCommand StandCommand(double time)
        {
            return SimpleMovementCommand.Stand(time);
        }

        public SimpleMovementCommand Move(double distance)
        {
            return SimpleMovementCommand.Move(
                Math.Sign(distance) * LinearVelocityLimit,
                Math.Abs(distance / LinearVelocityLimit)
            );
        }

        public SimpleMovementCommand Rotate(Angle angle)
        {
            return SimpleMovementCommand.Rotate(
                Math.Sign(angle.Grad) * AngularVelocityLimit,
                Math.Abs(angle / AngularVelocityLimit)
            );
        }

        public SimpleMovementCommand ActionCommand(string action)
        {
            return SimpleMovementCommand.Action(action);
        }

        public SimpleMovementCommand SleepCommand(double time)
        {
            return SimpleMovementCommand.Move(0, time);
        }
    }

    public interface ISimpleMovementWorld
    {
        SimpleMovementCommandHelper CommandHelper { get; }
    }

}