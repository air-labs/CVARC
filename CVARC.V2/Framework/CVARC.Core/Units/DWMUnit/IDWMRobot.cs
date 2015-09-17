using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public class EncodersData
	{
		public double Timestamp { get; set; }
		public Angle TotalLeftRotation { get; set; }
		public Angle TotalRightRotation { get; set; }
	}

	public class DWMData
	{
		public readonly List<EncodersData> EncodersHistory = new List<EncodersData>();
	}

	public interface IDWMRobot : IActor
	{
		DWMData DWMData { get; set; }
	}
}
