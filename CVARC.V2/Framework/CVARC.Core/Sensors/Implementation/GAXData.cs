using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public class GAXData
	{
		public double TimeStamp { get; set; }
		public Point3D Accelerations { get; set; }
		public Angle VelocityAroundX { get; set; }
		public Angle VelocityAroundY { get; set; }
		public Angle VelocityAroundZ { get; set; }
	}
}
