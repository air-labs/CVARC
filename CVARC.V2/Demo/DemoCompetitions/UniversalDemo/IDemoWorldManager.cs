using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
	public interface IDemoWorldManager : IWorldManager
	{
		void CreateObject(DemoObjectData data);
	}
}
