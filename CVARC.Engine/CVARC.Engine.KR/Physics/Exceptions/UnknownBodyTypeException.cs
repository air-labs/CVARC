namespace CVARC.Physics.Exceptions
{
	public class UnknownBodyTypeException : PhysicsException
	{
		public UnknownBodyTypeException()
		{
		}

		public UnknownBodyTypeException(string message) : base(message)
		{			
		}
	}
}
