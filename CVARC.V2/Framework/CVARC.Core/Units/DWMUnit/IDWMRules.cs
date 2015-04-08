using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public interface IDWMRules
	{
		double WheelRadius { get; set; }
		double DistanceBetweenWheels { get; set; }
		Angle RotationSpeedLimit { get; set; }
	}

	public interface IDWMRules<TCommand> : IDWMRules
		where TCommand : IDWMCommand
	{

	}

}
