using AIRLab.Mathematics;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public delegate void DWMTestEntry(CvarcClient<DWMSensorsData, DWMCommand> client, DWMWorld world, IAsserter asserter);
    public class DWMMovementTestBase : DWMTestBase
    {
        public DWMMovementTestBase(DWMTestEntry entry)
            : base(entry, KnownDWMWorldStates.NoDistortion) { }
    }

	public class DWMDistortionTestBase : DWMTestBase
	{
		public DWMDistortionTestBase(DWMTestEntry entry)
			: base(entry, KnownDWMWorldStates.HardDistortion) { }
	}

	public class KnownDWMWorldStates
	{
		public static DWMWorldState NoDistortion = new DWMWorldState 
		{
			Multiplier=1,
			  Robots = 
				{
					new DemoRobotData
					{
						 Color= ObjectColor.Red,
						 IsRound=true,
						 RobotName=TwoPlayersId.Left,
						 X=0,
						 Y=0,
						 YSize=5,
						 XSize=5,
						 ZSize=10
					}
				}
		};
		public static DWMWorldState HardDistortion = new DWMWorldState 
		{
			Multiplier=1,
			  Robots = 
				{
					new DemoRobotData
					{
						 Color= ObjectColor.Red,
						 IsRound=true,
						 RobotName=TwoPlayersId.Left,
						 X=0,
						 Y=0,
						 YSize=5,
						 XSize=5,
						 ZSize=10
					}
				}
		};

	}

    public class DWMTestBase : DelegatedCvarcTest<DWMSensorsData, DWMCommand, DWMWorld, DWMWorldState>
    {
        public override SettingsProposal GetSettings()
        {
            return new SettingsProposal
            {
                TimeLimit = 10,
                Controllers = new List<ControllerSettings> 
                    {
                        new ControllerSettings  { ControllerId=TwoPlayersId.Left, Name="This", Type= ControllerType.Client},
                    }
            };
        }

        DWMWorldState WorldState;

		public override DWMWorldState GetWorldState()
        {
            return WorldState;
        }

		public DWMTestBase(DWMTestEntry entry, DWMWorldState state)
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
