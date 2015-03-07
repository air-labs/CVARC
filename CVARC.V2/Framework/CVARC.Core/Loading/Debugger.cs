using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public static class Debugger
	{
		public static void Log(string str)
		{
			if (Logger!=null)
				Logger(str);
		}
		public static Action<string> Logger;
	}
}
