using AIRLab.Mathematics;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepairTheStarship
{
	public class RTSRules
	{
		public static readonly MoveAndGripRules Current;
		static RTSRules()
		{
			Current = new MoveAndGripRules(90, Angle.FromGrad(90), 1, 1);
		}
	}
}
