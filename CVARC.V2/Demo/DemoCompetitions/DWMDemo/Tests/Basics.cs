using AIRLab.Mathematics;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public delegate void DWMTestEntry(CvarcClient<DWMSensorsData, DWMCommand> client, DemoWorld world, IAsserter asserter);
    public class DWMMovementTestBase : DWMTestBase
    {
        public DWMMovementTestBase(DWMTestEntry entry)
            : base(entry, KnownWorldStates.EmptyWithOneRobot(false)) { }
    }
    public class DWMTestBase : DelegatedCvarcTest<DWMSensorsData, DWMCommand, DemoWorld, DemoWorldState>
    {
        public override SettingsProposal GetSettings()
        {
            return new SettingsProposal
            {
                TimeLimit = 10,
                Controllers = new List<ControllerSettings> 
                    {
                        new ControllerSettings  { ControllerId=TwoPlayersId.Left, Name="This", Type= ControllerType.Client},
                        new ControllerSettings  { ControllerId=TwoPlayersId.Right, Name="Stand", Type= ControllerType.Bot}
                    }
            };
        }

        DemoWorldState WorldState;

        public override DemoWorldState GetWorldState()
        {
            return WorldState;
        }

        public DWMTestBase(DWMTestEntry entry, DemoWorldState state)
            : base((client, world, asserter) => { entry(client, world, asserter); })
        {
            WorldState = state;
        }
    }


    partial class DWMLogicPartHelper
	{
		DWMTestEntry LocationTest(double X, double Y, double angleInGrad, double tolerance, params DWMCommand[] command)
		{
			return LocationTest(
				(frame, asserter) =>
				{
					asserter.IsEqual(X, frame.X, 1e-2 * tolerance);
					asserter.IsEqual(Y, frame.Y, 1e-2 * tolerance);
					asserter.IsEqual(angleInGrad, frame.Angle.Grad % 360, 1e-1 * tolerance);
				},
					command);

		}

		DWMTestEntry LocationTest(Action<Frame2D, IAsserter> test, params DWMCommand[] command)
		{
			return (client, world, asserter) =>
			{
				DWMSensorsData data = null;
				int x = 0;
				foreach (var c in command)
				{
					Debugger.Log(DebuggerMessageType.UnityTest, "Before performance");
					data = client.Act(c);
					Debugger.Log(DebuggerMessageType.UnityTest, String.Format("Performed: {0} {1}", c.ToString(), x++));
				}
				Debugger.Log(DebuggerMessageType.UnityTest, "All commands performed");
				test(new Frame2D(data.Locations[0].X, data.Locations[0].Y, Angle.FromGrad(data.Locations[0].Angle)), asserter);
			};
		}
	}
}
