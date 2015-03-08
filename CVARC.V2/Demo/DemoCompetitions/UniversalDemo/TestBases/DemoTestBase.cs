using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
	public delegate void DemoTestEntry(CvarcClient<DemoSensorsData, MoveAndGripCommand> client, DemoWorld world, IAsserter asserter);

	public class DemoTestBase : DelegatedCvarcTest<DemoSensorsData, MoveAndGripCommand, DemoWorld, DemoWorldState>
	{
		public override SettingsProposal GetSettings()
		{
			return new SettingsProposal
				{
					TimeLimit = 10,
					Controllers = new List<ControllerSettings> 
                    {
                        new ControllerSettings  { ControllerId=TwoPlayersId.Left, Name="This", Type= ControllerType.Client}
                    }
				};
		}

		DemoWorldState WorldState;

		public override DemoWorldState GetWorldState()
		{
			return WorldState;
		}

		public DemoTestBase(DemoTestEntry entry, DemoWorldState state)
			: base((client, world, asserter) => { entry(client, world, asserter); })
		{
			WorldState = state;
		}
	}

	
}
