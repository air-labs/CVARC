using AIRLab.Mathematics;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboMovies
{
	public class RMRules
	{
        public static readonly MoveAndBuildRules Current = new MoveAndBuildRules(90, Angle.FromGrad(90), 6, 1, 1);
	}
}
