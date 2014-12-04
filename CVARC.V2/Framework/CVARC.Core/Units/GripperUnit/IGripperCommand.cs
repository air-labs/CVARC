using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2.Units
{
	public interface IGripperCommand 
	{
		GripperAction GripperCommand { get; set; }
	}
}
