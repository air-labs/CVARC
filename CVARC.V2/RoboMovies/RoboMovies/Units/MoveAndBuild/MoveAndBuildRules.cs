using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public class MoveAndBuildRules : IRules, ITowerBuilderRules<MoveAndBuildCommand>, ISimpleMovementRules<MoveAndBuildCommand>
	{
		public void DefineKeyboardControl(IKeyboardController _pool, string controllerId)
		{
			var pool = Compatibility.Check<KeyboardController<MoveAndBuildCommand>>(this, _pool);
			this.AddBuilderKeys<MoveAndBuildCommand>(pool, controllerId);
			this.AddSimpleMovementKeys<MoveAndBuildCommand>(pool, controllerId);
            pool.StopCommand = () => new MoveAndBuildCommand { SimpleMovement = SimpleMovement.Stand(0.1) };
		}

        public double BuildingTime { get; private set; }
        public double CollectingTime { get; private set; }
        public int BuilderCapacity { get; private set; }
		
        public double LinearVelocityLimit { get; private set; }
		public AIRLab.Mathematics.Angle AngularVelocityLimit { get; private set; }

		public MoveAndBuildRules(double linearVelocityLimit, Angle angularVelocityLimit, 
            int builderCapacity, double collectingTime, double buildingTime)
		{
			LinearVelocityLimit = linearVelocityLimit;
			AngularVelocityLimit = angularVelocityLimit;
            BuilderCapacity = builderCapacity;
            CollectingTime = collectingTime;
            BuildingTime = buildingTime;
		}

		public MoveAndBuildRules() : this(50, Angle.HalfPi, 8, 1, 1) { }
    }
}
