using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public interface ISimpleMovementRules
	{
		double LinearVelocityLimit { get; }
		Angle AngularVelocityLimit { get; }
	}

	public interface ISimpleMovementRules<TCommand> : ISimpleMovementRules
		where TCommand : ISimpleMovementCommand
	{

	}

}