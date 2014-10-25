using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;

namespace CVARC.V2.SimpleMovement
{
    [Serializable]
    public class SimpleMovementCommand : ICommand
    {
        public double LinearVelocity { get; private set; }
        public Angle AngularVelocity { get; private set; }
        public double Duration { get; private set; }
        public string Command { get; private set; }
        public bool WaitForExit { get; private set; }

        public static SimpleMovementCommand Move(double linearVelocity, double duration)
        {
            return new SimpleMovementCommand { LinearVelocity = linearVelocity, Duration = duration };
        }

        public static SimpleMovementCommand Rotate(Angle angularVelocity, double duration)
        {
            return new SimpleMovementCommand { AngularVelocity = angularVelocity, Duration = duration };
        }

        public static SimpleMovementCommand Exit()
        {
            return new SimpleMovementCommand { WaitForExit = true };
        }

        public static SimpleMovementCommand Action(string action, double duration=0)
        {
            return new SimpleMovementCommand { Command = action, Duration=duration };
        }
    }
}
