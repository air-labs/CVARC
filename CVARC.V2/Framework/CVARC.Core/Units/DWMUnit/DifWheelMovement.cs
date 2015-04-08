using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CVARC.V2
{
	[DataContract]
	public class DifWheelMovement
	{
		[DataMember]
		public Angle LeftRotatingVelocity { get; set; }
		[DataMember]
		public Angle RightRotatingVelocity { get; set; }
		[DataMember]
		public double Duration { get; set; }
	}
}
