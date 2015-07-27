using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Demo
{
	[DataContract]
	public class DWMCommand :IDWMCommand, ICommand
	{

		[DataMember]
		public DifWheelMovement DifWheelMovement
		{
			get;
			set;
		}
	}
}
