using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2.Units
{
	public class MoveAndGripRules : IRules, IGripperRules<MoveAndGripCommand>, ISimpleMovementRules<MoveAndGripCommand>
	{
		public void DefineKeyboardControl(IKeyboardControllerPool _pool, string controllerId)
		{
			var pool = Compatibility.Check<KeyboardControllerPool<MoveAndGripCommand>>(this, _pool);
			this.AddGripKeys(pool, controllerId);
			this.AddSimpleMovementKeys(pool, controllerId);
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
	}
}
