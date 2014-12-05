using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2.Units
{
	public class MoveAndGripCommand : ICommand, ISimpleMovementCommand, IGripperCommand
	{

		public SimpleMovement SimpleMovement
		{
			get;
			set;
		}

		public GripperAction GripperCommand
		{
			get;
			set;
		}
	}
}
