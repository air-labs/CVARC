//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using AIRLab.Mathematics;
//using CVARC.V2;

//namespace Demo
//{
//	partial class InteractionLogicPartHelper
//	{


//		InteractionTestEntry InteractionTest(bool flag, params DemoCommand[] command)
//		{
//			return (client, world, asserter) =>
//			{
//				InteractionSensorData data = null;
//				foreach (var c in command)
//					data = client.Act(c);
//				//asserter.IsEqual(false, data != null);
//				asserter.IsEqual(flag, data.IsGripped);
//			};
//		}


//		void LoadTests(LogicPart logicPart, DemoRules rules)
//		{

//			logicPart.Tests["Grip"] = new InteractionTestBase(InteractionTest(true,
//				rules.Rotate(-Angle.HalfPi),
//				rules.Move(15),
//				rules.Grip(),
//				rules.Move(-10)), true);
//			logicPart.Tests["GripThroughWall"] = new InteractionTestBase(InteractionTest(false,
//				rules.Rotate(Angle.HalfPi),
//				rules.Move(50),
//				rules.Grip(),
//				rules.Move(-50)), true);
//			logicPart.Tests["Release"] = new InteractionTestBase(InteractionTest(false,
//				rules.Rotate(Angle.Pi*2),
//				rules.Move(15),
//				rules.Grip(),
//				rules.Move(-50),
//				rules.Release(),
//				rules.Rotate(Angle.Pi)), true);
//			logicPart.Tests["GripUnGripable"] = new InteractionTestBase(InteractionTest(false,
//				rules.Move(25),
//				rules.Rotate(Angle.HalfPi),
//				rules.Move(25),
//				rules.Grip(),
//				rules.Move(-25)), true);
//		}
//	}
//}
