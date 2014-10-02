using System.Collections.Generic;
using System.Linq;
using AIRLab.Mathematics;
using NUnit.Framework;

namespace CVARC.Core
{
	internal class LocationTests
	{
		[Test]
		public void TreeMovingConstantLocation()
		{
			var world = new Body();
			var boxLocation = new Frame3D(10, 0, 0);
			var ballLocation = new Frame3D(20, 10, 0);
			var box = new Box {Location = boxLocation};
			var ball = new Ball {Location = ballLocation};
			world.Add(box);
			world.Add(ball);
			box.DetachAttachMaintaingLoction(ball);
			Assert.AreEqual(boxLocation, box.Location);
			Assert.AreEqual(ballLocation, ball.GetAbsoluteLocation());
			Assert.AreEqual(new Frame3D(10, 10, 0), ball.Location);
			world.DetachAttachMaintaingLoction(ball);
			Assert.AreEqual(ballLocation, ball.Location);
			Assert.AreEqual(ballLocation, ball.GetAbsoluteLocation());
		}

		[Test]
		public void TreeMovingNoParent()
		{
			var oldBoxLocation = new Frame3D(10, 20, 30);
			var box = new Box
			          	{
			          		Location = oldBoxLocation
			          	};
			var newParent = new Body
			                	{
			                		Location = new Frame3D(20, 10, 20)
			                	};
			newParent.DetachAttachMaintaingLoction(box);
			Assert.AreEqual(oldBoxLocation, box.GetAbsoluteLocation());
			Assert.AreEqual(new Frame3D(-10, 10, 10), box.Location);
		}

		[Test]
		public void Location()
		{
			var world = new Body();
			var loc = new Frame3D(10, 0, 0);
			world.Add(new Box {Location = loc});
			Body box = world.Nested.First();
			Assert.AreEqual(loc, box.Location);
			Assert.AreEqual(loc, box.GetAbsoluteLocation());
			box.Add(new Ball {Location = new Frame3D(0, 0, 30)});
			Body ball = box.Nested.First();
			Assert.AreEqual(new Frame3D(0, 0, 30), ball.Location);
			Assert.AreEqual(new Frame3D(10, 0, 30), ball.GetAbsoluteLocation());
		}

		[Test]
		public void AbsoluteLocationLinear()
		{
			var root = new Body {Location = new Frame3D(0, 0, 10)};
			Assert.AreEqual(root.Location, root.GetAbsoluteLocation());
			var box = new Box {Location = new Frame3D(10, 0, 0)};
			Assert.AreEqual(box.Location, box.GetAbsoluteLocation());
			root.Add(box);
			Assert.AreEqual(box.Location.Apply(root.Location), box.GetAbsoluteLocation());
			int sum = 0;
			for(int i = 0; i < 10; i++)
			{
				sum += i;
				Body child = new Box {Location = new Frame3D(i, 0, 0)};
				root.Add(child);
				Assert.AreEqual(sum, child.GetAbsoluteLocation().X);
				root = child;
			}
		}

		[Test]
		public void AbsoluteLocationOrder1()
		{
			var child = GetTreeChildForTopDownLocations(new[]
				{
					new Frame3D(10, 0, 0),
					Frame3D.DoYaw(Angle.HalfPi),
				});
			CheckFrameIsWithin(new Frame3D(10, 0, 0, Angle.Zero, Angle.HalfPi, Angle.Zero), child.GetAbsoluteLocation());
			child = GetTreeChildForTopDownLocations(
				new[]
					{
						Frame3D.DoYaw(Angle.HalfPi),
						new Frame3D(10, 0, 0)
					});
			CheckFrameIsWithin(new Frame3D(0, 10, 0, Angle.Zero, Angle.HalfPi, Angle.Zero), child.GetAbsoluteLocation());
		}
		[Test]
		public void AbsoluteLocationOrder2()
		{
			var pitch = Angle.FromGrad(30);
			var child = GetTreeChildForTopDownLocations(
				new[]
					{
						Frame3D.DoPitch(pitch),
						new Frame3D(4,0, 0),
					});
			CheckFrameIsWithin(new Frame3D(4*Geometry.Cos(pitch),0,-4*Geometry.Sin(pitch)), child.GetAbsoluteLocation());
			child = GetTreeChildForTopDownLocations(
				new[]
					{
						new Frame3D(4, 0, 0),
						Frame3D.DoPitch(pitch),

					});
			CheckFrameIsWithin(new Frame3D(4 , 0, 0,Angle.Zero,pitch,Angle.Zero), child.GetAbsoluteLocation());
		}

		private static Body GetTreeChildForTopDownLocations(IEnumerable<Frame3D> frames)
		{
			var currentChild = new Body();
			foreach(var frame3D in frames)
			{
				var newChild = new Body {Location = frame3D};
				currentChild.Add(newChild);
				currentChild = newChild;
			}
			return currentChild;
		}
		private static void CheckFrameIsWithin(Frame3D expected, Frame3D actual, double epsilon = 0.01)
		{
			Assert.Less(Geometry.Hypot(expected-actual),epsilon, "Expected {0} but was {1}", expected,actual);	
		}
	}
}