using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CVARC.V2
{
    [DataContract]
	public class MoveAndGripCommand : ICommand, ISimpleMovementCommand, IGripperCommand
	{
        [DataMember]
		public SimpleMovement SimpleMovement
		{
			get;
			set;
		}

        [DataMember]
		public GripperAction GripperCommand
		{
			get;
			set;
		}
	}
}
