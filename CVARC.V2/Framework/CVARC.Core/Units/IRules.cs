using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public interface IRules 
	{
		void DefineKeyboardControl(IKeyboardController keyboardController, string controllerId);
       
	}
}
