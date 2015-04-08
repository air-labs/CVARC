//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using AIRLab.Mathematics;
//using CVARC.V2;

//namespace Demo.Collision
//{
//	public partial class CollisionLogicPartHelper
//	{
//		DemoTestEntry LocationTest(int count, int left, int right, params DemoCommand[] command)
//		{
//			return (client, world, asserter) =>
//			{
//				int counter = 0;
//				int lscore = 0;
//				int rscore = 0;
//				world.Scores.ScoresChanged += () => {counter++;};
//				foreach (var c in command)
//					client.Act(c);
//				asserter.IsEqual(count, counter,0);
//				foreach (var item in world.Scores.Records)
//				{
//					if (item.Key == "Left")
//						lscore = item.Value.Count;
//					if (item.Key == "Right")
//						rscore = item.Value.Count;
//				}
//				asserter.IsEqual(lscore, left, 0);
//				asserter.IsEqual(rscore, right, 0);
//			};
//		}


//		void LoadTests(LogicPart logicPart, DemoRules rules)
//		{
//			logicPart.Tests["NoCollision"] = new CollisionTestBase(LocationTest(0, 0, 0,
//				rules.Grip(),
//				rules.MoveWithVelocityForTime(-50, 0.2),
//				rules.MoveWithVelocityForTime(50, 0.2)), true);
//			logicPart.Tests["CollisionCount"] = new CollisionTestBase(LocationTest(3, 3, 0,
//				rules.Grip(),
//				rules.MoveWithVelocityForTime(50, 1),
//				rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
//				rules.MoveWithVelocityForTime(50, 0.8),
//				rules.MoveWithVelocityForTime(50, 0.3),
//				rules.MoveWithVelocityForTime(-50, 0.2),
//				rules.MoveWithVelocityForTime(50, 0.3),
//				rules.MoveWithVelocityForTime(-50, 0.2),
//				rules.MoveWithVelocityForTime(50, 0.3),
//				rules.MoveWithVelocityForTime(-50, 0.2)), true);
//			logicPart.Tests["CollisionBox"] = new CollisionTestBase(LocationTest(1, 1, 0,
//				rules.Grip(),
//				rules.MoveWithVelocityForTime(50, 1.5),
//				rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
//				rules.MoveWithVelocityForTime(50, 0.45)), true);
//			logicPart.Tests["NoBox"] = new CollisionTestBase(LocationTest(3, 3, 0,
//				rules.MoveWithVelocityForTime(50, 1.05),
//				rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
//				rules.MoveWithVelocityForTime(50, 0.8),
//				rules.MoveWithVelocityForTime(50, 0.3),
//				rules.MoveWithVelocityForTime(-50, 0.2),
//				rules.MoveWithVelocityForTime(50, 0.3),
//				rules.MoveWithVelocityForTime(-50, 0.2),
//				rules.MoveWithVelocityForTime(50, 0.3),
//				rules.MoveWithVelocityForTime(-50, 0.2)), true);
//			logicPart.Tests["RotateNoBox"] = new CollisionTestBase(LocationTest(1, 1, 0,
//				rules.MoveWithVelocityForTime(50, 1.7),
//				rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
//				rules.MoveWithVelocityForTime(50, 1.45),
//				rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
//				rules.MoveWithVelocityForTime(50, 0.7),
//				rules.RotateWithVelocityForTime(Angle.HalfPi, 1)), true);
//			logicPart.Tests["RotateBox"] = new CollisionTestBase(LocationTest(1, 1, 0,
//				rules.Grip(),
//				rules.MoveWithVelocityForTime(50, 1.7),
//				rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
//				rules.MoveWithVelocityForTime(50, 1.6),
//				rules.RotateWithVelocityForTime(Angle.HalfPi, 1),
//				rules.MoveWithVelocityForTime(50, 0.7),
//				rules.RotateWithVelocityForTime(Angle.HalfPi, 1)), true);



//		}
//	}
//}
