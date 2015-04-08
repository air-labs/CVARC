using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Demo
{
	[DataContract]
	public class DemoCommand : IGripperCommand, ISimpleMovementCommand, IDWMCommand
	{
		[DataMember]
		public GripperAction GripperCommand
		{
			get;
			set;
		}

		[DataMember]
		public SimpleMovement SimpleMovement
		{
			get;
			set;
		}

		[DataMember]
		public DifWheelMovement DifWheelMovement
		{
			get;
			set;
		}
	}
}
