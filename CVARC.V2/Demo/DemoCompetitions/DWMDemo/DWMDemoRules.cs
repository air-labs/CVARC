using AIRLab.Mathematics;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
	public class DWMRules : IRules, IDWMRules<DWMCommand>
	{
	


		public double WheelRadius
		{
			get;
			set;
		}

		public double DistanceBetweenWheels
		{
			get;
			set;
		}

		public AIRLab.Mathematics.Angle RotationSpeedLimit
		{
			get;
			set;
		}

		public DWMRules()
		{
			WheelRadius = 5;
			DistanceBetweenWheels = 10;
			RotationSpeedLimit = 2 * Angle.Pi;
		}

		public void DefineKeyboardControl(IKeyboardController _pool, string controllerId)
		{
			var pool = Compatibility.Check<KeyboardController<DemoCommand>>(this, _pool);

			pool.StopCommand = () => new DemoCommand { SimpleMovement = SimpleMovement.Stand(0.1) };
		}


		public double LinearVelocityLimit
		{
			get;
			set;
		}

		public Angle AngularVelocityLimit
		{
			get;
			set;
		}
	}
}
