using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public interface ISimpleMovementCommand : ICommand
	{
		SimpleMovement SimpleMovement { get; set; }
	}

}
