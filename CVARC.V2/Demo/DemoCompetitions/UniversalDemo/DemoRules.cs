using AIRLab.Mathematics;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Demo
{
	public class DemoRules : IRules, IGripperRules<DemoCommand>, ISimpleMovementRules<DemoCommand>, IDWMRules<DemoCommand>
	{
		public double GrippingTime
		{
			get;
			set;
		}

		public double ReleasingTime
		{
			get;
			set;
		}

		public double LinearVelocityLimit
		{
			get;
			set;
		}

		public AIRLab.Mathematics.Angle AngularVelocityLimit
		{
			get;
			set;
		}



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

		public DemoRules()
		{
			LinearVelocityLimit = 50;
			AngularVelocityLimit = Angle.HalfPi;
			GrippingTime = ReleasingTime = 1;
			WheelRadius = 5;
			DistanceBetweenWheels = 10;
			RotationSpeedLimit = 2 * Angle.Pi;
		}

		public void DefineKeyboardControl(IKeyboardController _pool, string controllerId)
		{
			var pool = Compatibility.Check<KeyboardController<DemoCommand>>(this, _pool);

			this.AddGripKeys(pool, controllerId);
			this.AddSimpleMovementKeys(pool, controllerId);
			pool.StopCommand = () => new DemoCommand { SimpleMovement = SimpleMovement.Stand(0.1) };
		}
	}
}
