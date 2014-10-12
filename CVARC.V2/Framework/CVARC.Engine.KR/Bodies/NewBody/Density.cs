namespace CVARC.Core
{

	public class Density
	{
		private Density(double value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return string.Format("Value: {0}", Value);
		}

		public double Value { get; private set; }
		public static readonly Density Wood=new Density(0.9);
		public static readonly Density Water=new Density(1);
		public static readonly Density Iron=new Density(7.8);
		public static readonly Density Aluminum=new Density(2.7);
		public static readonly Density PlasticPvc=new Density(1.3);
		public static readonly Density None=new Density(0);
	}
}
