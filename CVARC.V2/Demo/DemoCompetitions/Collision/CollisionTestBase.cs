//using CVARC.V2;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Demo
//{
   
//	public class CollisionTestBase : DelegatedCvarcTest<SensorsData, DemoCommand, MovementWorld, MovementWorldState>
//	{
//		public override SettingsProposal GetSettings()
//		{
//			return new SettingsProposal
//			{
//				TimeLimit = 10,
//				Controllers = new List<ControllerSettings> 
//					{
//						new ControllerSettings  { ControllerId="Left", Name="This", Type= ControllerType.Client},
//						new ControllerSettings  { ControllerId="Right", Name="Grip", Type= ControllerType.Bot}
//					}
//			};
//		}
//		bool RobotIsRectangular;

//		public override MovementWorldState GetWorldState()
//		{
//			return new MovementWorldState() { RectangularRobot = RobotIsRectangular };
//		}

//		public CollisionTestBase(MovementTestEntry entry, bool robotIsRectangular = false)
//			: base((client, world, asserter) => { entry(client, world, asserter); })
//		{
//			RobotIsRectangular = robotIsRectangular;
//		}
//	}
//}
