using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public class MoveAndGripRules : IRules, IGripperRules<MoveAndGripCommand>, ISimpleMovementRules<MoveAndGripCommand>
	{
		public void DefineKeyboardControl(IKeyboardController _pool, string controllerId)
		{
			var pool = Compatibility.Check<KeyboardController<MoveAndGripCommand>>(this, _pool);
			this.AddGripKeys(pool, controllerId);
			this.AddSimpleMovementKeys(pool, controllerId);
            pool.StopCommand = () => new MoveAndGripCommand { SimpleMovement = SimpleMovement.Stand(0.1) };
		}

		public double LinearVelocityLimit
		{
			get;
			private set; 
		}

		public AIRLab.Mathematics.Angle AngularVelocityLimit
		{
			get;
			private set;
		}

		public MoveAndGripRules(double linearVelocityLimit, Angle angularVelocityLimit, double grippingTime, double releasingTime)
		{
			LinearVelocityLimit = linearVelocityLimit;
			AngularVelocityLimit = angularVelocityLimit;
            GrippingTime = grippingTime;
            ReleasingTime = releasingTime;
		}

		public MoveAndGripRules() : this(50, Angle.HalfPi,1,1) { }

        public double GrippingTime
        {
            get;
            private set;
        }

        public double ReleasingTime
        {
            get;
            private set;
        }
    }
}
