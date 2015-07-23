using AIRLab.Mathematics;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
	partial class DWMLogicPartHelper
	{

		DemoTestEntry LocationTest(double X, double Y, double angleInGrad, double tolerance, params DemoCommand[] command)
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

		DemoTestEntry LocationTest(Action<Frame2D, IAsserter> test, params DemoCommand[] command)
		{
			return (client, world, asserter) =>
			{
				DemoSensorsData data = null;
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
