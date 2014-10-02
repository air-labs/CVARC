using System.Linq;
using AIRLab.Mathematics;
using NUnit.Framework;

namespace CVARC.Core.Replay
{
	public partial class LoggingObject
	{
		public class SavingBodiesTests
		{
			[SetUp]
			public void SetUp()
			{
				_totalTime = 0;
				_movementCount = 0;
			}

			[Test]
			public void ZeroTime()
			{
				var root = new Body();
				var lo = new LoggingObject(root, root);
				lo.SaveBody(Frame3D.Identity, 0);
				Assert.AreEqual(1,lo.Movements.Count);
				Assert.AreEqual(1,lo.VisibilityStates.Count);
			}
			[Test]
			public void TestSaveLocation1()
			{
				var root = new Body
				           	{
				           		Location = new Frame3D(10, 0, 0)
				           	};	
				Body box = new Box
				           	{
				           		Location = new Frame3D(10, 20, 30)
				           	};
				var lo = new LoggingObject(box, root);
				Assert.AreEqual(box, lo.Body);
				MoveAndCheck(lo, box);
				//now stop
				for(int i = 0; i < 3; i++)
					lo.SaveLocation(box.GetAbsoluteLocation(), 0.1);
				Assert.AreEqual(1, lo.Movements.Count);
				//move
				MoveAndCheck(lo, box);
			}

			[Test]
			public void TestSaveVisibility()
			{
				var root = new Body
				           	{
				           		Location = new Frame3D(10, 0, 0)
				           	};
				Body box = new Box
				           	{
				           		Location = new Frame3D(10, 20, 30)
				           	};
				root.Add(box);
				var lo = new LoggingObject(box, root);
				lo.SaveVisibilityState(_totalTime);
				Assert.AreEqual(1, lo.VisibilityStates.Count);
				Assert.AreEqual(_totalTime, lo.VisibilityStates[0].StartTime);
				_totalTime ++;
				lo.SaveVisibilityState(_totalTime);
				Assert.AreEqual(1, lo.VisibilityStates.Count);
				Assert.AreEqual(true, lo.VisibilityStates[0].IsVisible);
				root.Remove(box);
				_totalTime++;
				lo.SaveVisibilityState(_totalTime);
				Assert.AreEqual(_totalTime, lo.VisibilityStates.Last().StartTime);
				Assert.AreEqual(2, lo.VisibilityStates.Count);
				Assert.AreEqual(false, lo.VisibilityStates.Last().IsVisible);
			}

			[Test]
			public void TestSavingVisibility2()
			{
				var root = new Body
				           	{
				           		Location = new Frame3D(10, 0, 0)
				           	};
				Body box = new Box
				           	{
				           		Location = new Frame3D(10, 20, 30)
				           	};
				var lo = new LoggingObject(box, root);
				for (int i = 0; i < 100; i++)
				{
					lo.SaveVisibilityState(i);
					Assert.AreEqual(false, lo.VisibilityStates.Last().IsVisible);
					Assert.AreEqual(0, lo.VisibilityStates.Last().StartTime);
				}
				//body suddenly appears.
				const int updateTime = 100;
				root.Add(box);
				lo.SaveVisibilityState(updateTime);
				Assert.AreEqual(2, lo.VisibilityStates.Count);
				Assert.AreEqual(true, lo.VisibilityStates.Last().IsVisible);
				Assert.AreEqual(updateTime, lo.VisibilityStates.Last().StartTime);
			}

			private static void MoveAndCheck(LoggingObject lo, Body box)
			{
				for(int i = 0; i < 5; i++)
				{
					box.Location = box.Location.Apply(new Frame3D(1, 1, 0));
					lo.SaveLocation(box.GetAbsoluteLocation(), _totalTime);
					Frame3D loc = lo.Movements.Last().NextLocation();
					Assert.AreEqual(box.GetAbsoluteLocation(), loc);
				}
				Assert.AreEqual(_totalTime, lo.Movements[_movementCount].StartTime);
				_totalTime += 0.1;
				_movementCount++;
				Assert.AreEqual(_movementCount, lo.Movements.Count);
			}

			private static double _totalTime;
			private static int _movementCount;
		}
	}
}