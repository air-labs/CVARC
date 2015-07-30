using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public class RMRules : IRules, ITowerBuilderRules<RMCommand>, ISimpleMovementRules<RMCommand>, IGripperRules<RMCommand>, IRMCombinedRules<RMCommand>
	{
        public static readonly RMRules Current = new RMRules();
		
        public void DefineKeyboardControl(IKeyboardController _pool, string controllerId)
		{
			var pool = Compatibility.Check<KeyboardController<RMCommand>>(this, _pool);
			this.AddBuilderKeys<RMCommand>(pool, controllerId);
			this.AddSimpleMovementKeys<RMCommand>(pool, controllerId);
            this.AddGripKeys<RMCommand>(pool, controllerId);
            this.AddCombinedKeys<RMCommand>(pool, controllerId);
            pool.StopCommand = () => new RMCommand { SimpleMovement = SimpleMovement.Stand(0.1) };
		}

        public double BuildingTime { get; private set; }
        public double CollectingTime { get; private set; }
        public int BuilderCapacity { get; private set; }

        public double GrippingTime { get; private set; }
        public double ReleasingTime { get; private set; }
		
        public double LinearVelocityLimit { get; private set; }
		public AIRLab.Mathematics.Angle AngularVelocityLimit { get; private set; }

		public RMRules(double linearVelocityLimit, Angle angularVelocityLimit, 
            int builderCapacity, double collectingTime, double buildingTime, 
            double grippingTime, double releasingTime)
		{
			LinearVelocityLimit = linearVelocityLimit;
			AngularVelocityLimit = angularVelocityLimit;
            BuilderCapacity = builderCapacity;
            CollectingTime = collectingTime;
            BuildingTime = buildingTime;
            GrippingTime = grippingTime;
            ReleasingTime = releasingTime;
		}

		public RMRules() : this(50, Angle.HalfPi, 8, 1, 1, 1, 1) { }
    }
}
