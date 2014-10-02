using System;

namespace CVARC.Physics.Exceptions
{
	public class PhysicsException : Exception
	{
		public PhysicsException()
		{
		}

		public PhysicsException(string message) : base(message)
		{			
		}
	}
}
