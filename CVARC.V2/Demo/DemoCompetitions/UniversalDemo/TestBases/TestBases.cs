using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
	public class RoundMovementTestBase : DemoTestBase
	{
		public RoundMovementTestBase(DemoTestEntry entry)
			: base(entry, KnownWorldStates.EmptyWithOneRobot(false))
		{ }
	}

	public class RectangularMovementTestBase : DemoTestBase
	{
		public RectangularMovementTestBase(DemoTestEntry entry)
			: base(entry, KnownWorldStates.EmptyWithOneRobot(true))
		{ }
	}

	public class RectangularInteractionTestBase : DemoTestBase
	{
		public RectangularInteractionTestBase(DemoTestEntry entry)
			: base(entry, KnownWorldStates.InteractionScene(true))
		{ }
	}

		public class RoundInteractionTestBase : DemoTestBase
	{
			public RoundInteractionTestBase(DemoTestEntry entry)
			: base(entry, KnownWorldStates.InteractionScene(false))
		{ }
	}

        public class RectangularGrippingTestBase : DemoTestBase
        {
            public RectangularGrippingTestBase(DemoTestEntry entry)
                : base(entry, KnownWorldStates.InteractionScene(true))
            { }
        }

        public class RoundGrippingTestBase : DemoTestBase
        {
            public RoundGrippingTestBase(DemoTestEntry entry)
                : base(entry, KnownWorldStates.InteractionScene(false))
            { }
        }

}
