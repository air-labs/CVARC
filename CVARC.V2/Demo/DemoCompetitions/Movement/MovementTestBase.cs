using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
	public delegate void MovementTestEntry(CvarcClient<SensorsData, MoveAndGripCommand> client, MovementWorld world, IAsserter asserter);

	public class MovementTestBase : DelegatedCvarcTest<SensorsData, MoveAndGripCommand, MovementWorld, MovementWorldState>
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
		bool RobotIsRectangular;
		bool ObjectsOnField;
		public override MovementWorldState GetWorldState()
		{
			return new MovementWorldState()
			{
				RectangularRobot = RobotIsRectangular,
				objects = ObjectsOnField
			};
		}

		public MovementTestBase(MovementTestEntry entry,
			bool robotIsRectangular = false,
			bool objectsOnField = false)
			: base((client, world, asserter) => { entry(client, world, asserter); })
		{
			RobotIsRectangular = robotIsRectangular;
			ObjectsOnField = objectsOnField;
		}
	}
}
