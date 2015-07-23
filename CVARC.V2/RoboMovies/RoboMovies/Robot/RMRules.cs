using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public class RMRules : IRules, ITowerBuilderRules<RMCommand>, ISimpleMovementRules<RMCommand>
	{
        public static readonly RMRules Current = new RMRules(90, Angle.FromGrad(90), 6, 1, 1);
		
        public void DefineKeyboardControl(IKeyboardController _pool, string controllerId)
		{
			var pool = Compatibility.Check<KeyboardController<RMCommand>>(this, _pool);
			this.AddBuilderKeys<RMCommand>(pool, controllerId);
			this.AddSimpleMovementKeys<RMCommand>(pool, controllerId);
            pool.StopCommand = () => new RMCommand { SimpleMovement = SimpleMovement.Stand(0.1) };
		}

        public double BuildingTime { get; private set; }
        public double CollectingTime { get; private set; }
        public int BuilderCapacity { get; private set; }
		
        public double LinearVelocityLimit { get; private set; }
		public AIRLab.Mathematics.Angle AngularVelocityLimit { get; private set; }

		public RMRules(double linearVelocityLimit, Angle angularVelocityLimit, 
            int builderCapacity, double collectingTime, double buildingTime)
		{
			LinearVelocityLimit = linearVelocityLimit;
			AngularVelocityLimit = angularVelocityLimit;
            BuilderCapacity = builderCapacity;
            CollectingTime = collectingTime;
            BuildingTime = buildingTime;
		}

		public RMRules() : this(50, Angle.HalfPi, 8, 1, 1) { }
    }
}
