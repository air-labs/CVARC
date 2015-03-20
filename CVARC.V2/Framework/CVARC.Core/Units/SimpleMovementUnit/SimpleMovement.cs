using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AIRLab.Mathematics;

namespace CVARC.V2
{
    [Serializable]
    [DataContract]
    public class SimpleMovement
	{
        [DataMember]
        public double LinearVelocity { get; private set; }
        [DataMember]
        public Angle AngularVelocity { get; private set; }
        [DataMember]
        public double Duration { get; private set; }

		public static SimpleMovement Move(double linearVelocity, double duration)
		{
			return new SimpleMovement { LinearVelocity = linearVelocity, Duration = duration };
		}

		public static SimpleMovement MoveWithVelocity(double path, double linearVelocity)
		{
			return Move(Math.Sign(path) * linearVelocity, Math.Abs(path / linearVelocity));
		}

		public static SimpleMovement Rotate(Angle angularVelocity, double duration)
		{
			return new SimpleMovement { AngularVelocity = angularVelocity, Duration = duration };
		}

		public static SimpleMovement RotateWithVelocity(Angle angle, Angle angularVelocity)
		{
			return Rotate(Math.Sign(angle.Grad) * angularVelocity, Math.Abs(angle.Grad / angularVelocity.Grad));
		}

		public static SimpleMovement Stand(double time)
		{
			return new SimpleMovement { LinearVelocity = 0, AngularVelocity = Angle.Zero, Duration = time };
		}
    }
}
