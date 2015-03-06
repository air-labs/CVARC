using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using System.Runtime.Serialization;

namespace CVARC.V2.SimpleMovement
{
    [Serializable]
	[DataContract]
    public class SimpleMovementCommand : ICommand
    {
		[DataMember]
        public double LinearVelocity { get; private set; }
		[DataMember]
		public Angle AngularVelocity { get; private set; }
		[DataMember]
		public double Duration { get; private set; }
		[DataMember]
		public string Command { get; private set; }

        public static SimpleMovementCommand Move(double linearVelocity, double duration)
        {
            return new SimpleMovementCommand { LinearVelocity = linearVelocity, Duration = duration };
        }

        public static SimpleMovementCommand MoveWithVelocity(double path, double linearVelocity)
        {
            return Move(Math.Sign(path) * linearVelocity, Math.Abs(path / linearVelocity));
        }

        public static SimpleMovementCommand Rotate(Angle angularVelocity, double duration)
        {
            return new SimpleMovementCommand { AngularVelocity = angularVelocity, Duration = duration };
        }

        public static SimpleMovementCommand RotateWithVelocity(Angle angle, Angle angularVelocity)
        {
            return Rotate(Math.Sign(angle.Grad) * angularVelocity, Math.Abs(angle.Grad / angularVelocity.Grad));
        }

        public static SimpleMovementCommand Stand(double time)
        {
            return new SimpleMovementCommand { LinearVelocity = 0, AngularVelocity = Angle.Zero, Duration = time };
        }

        public static SimpleMovementCommand Action(string action, double duration=0)
        {
            return new SimpleMovementCommand { Command = action, Duration=duration };
        }
    }
}
