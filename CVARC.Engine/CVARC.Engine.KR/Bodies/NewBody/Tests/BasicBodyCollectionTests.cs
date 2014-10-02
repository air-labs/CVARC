using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CVARC.Core
{
	class BasicBodyCollectionTests
	{
		[Test]
		public void BodyEquality()
		{
			var box1 = new Box {XSize = 10, YSize = 20, ZSize = 30};
			var box2 = new Box {XSize = 10, YSize = 20, ZSize = 30};
			Assert.That(!box1.Equals(box2));
			var b1 = new Body {Model = new Model {FilePath = "sss"}};
			var b2 = new Body {Model = new Model {FilePath = "sss"}};
			Assert.That(!b1.Equals(b2));
		}

		[Test]
		public void AddRemove()
		{
			var box = new Body();
			var ball = new Ball();
			Assert.Null(ball.Parent);
			bool added = false;
			box.ChildAdded += b =>
				{
					Assert.AreEqual(ball, b);
					added = true;
				};
			box.Add(ball);
			Assert.AreEqual(box,ball.Parent);
			Assert.That(box.Nested.Contains(ball));
			Assert.That(added);

			bool removed = false;
			box.ChildRemoved += b =>
				{
					Assert.AreEqual(ball, b);
					removed = true;
				};
			box.Remove(ball);
			Assert.AreEqual(null, ball.Parent);
			Assert.That(!box.Nested.Contains(ball));
			Assert.That(removed);
		}
		[Test]
		public void DoubleOperations()
		{
			var root = new Body();
			var b = new Ball();
			root.Add(b);
			Assert.DoesNotThrow(() => root.Add(b));
			root.Remove(b);
			Assert.DoesNotThrow(() => root.Remove(b));
		}

		[Test]
		public void Clearing()
		{
			var root = new Body();
			var list = new List<Body>
			           	{
			           		new Box(),
			           		new Ball(),
			           		new Cylinder()
			           	};
			list.ForEach(root.Add);
			Assert.AreEqual(list.Count, root.Nested.Count());
			root.ChildRemoved += b =>
				{
					Assert.That(list.Contains(b));
					list.Remove(b);
					Assert.That(b.Parent==null);
				};
			root.Clear();
			Assert.AreEqual(0,root.Nested.Count());
			Assert.AreEqual(0,list.Count);
		}

		[Test]
		public void TreeRoot()
		{
			var root = new Body();
			Assert.AreEqual(root, root.TreeRoot);
			var box = new Box();
			root.Add(box);
			Assert.AreEqual(root, box.TreeRoot);
			root.Remove(box);
			Assert.AreEqual(root, box.TreeRoot);
			var ball = new Ball {new Ball(), new Ball(), new Ball {new Box()}};
			root.Add(ball);
			foreach(var child in root.GetSubtreeChildrenFirst())
				Assert.AreEqual(root, child.TreeRoot);
			root.Remove(ball);
			foreach (var child in ball.GetSubtreeChildrenFirst())
				Assert.AreEqual(ball, child.TreeRoot);
		}

		[Test]
		public void InvalidOperations()
		{
			var root = new Body();
			Assert.Throws<ArgumentNullException>(()=>root.Add(null));
			Assert.Throws<ArgumentNullException>(()=>root.Remove(null));
			var ball = new Ball();
			root.Add(ball);
			Assert.Throws<InvalidOperationException>(()=>new Body().Add(ball));
			Assert.DoesNotThrow(()=>root.Remove(new Body()));
		}

		[Test]
		public void MovingArbitrarilyInTree()
		{
			var childOne=new Body();
			var childTwo=new Body();
			var tree = new Body {new Box(), new Cylinder(), childOne};
			tree.Nested.First().Add(new Body {new Body {childTwo}});

			var oldParent = childTwo.Parent;
			childOne.DetachAttachMaintaingLoction(childTwo);
			Assert.AreEqual(childOne, childTwo.Parent);
			Assert.That(childOne.Contains(childTwo));
			Assert.That(!oldParent.Contains(childTwo));
			childOne.Parent.Remove(childOne);

			Assert.AreEqual(null, childOne.Parent);
			tree.DetachAttachMaintaingLoction(childOne);
			Assert.AreEqual(tree, childOne.Parent);
			Assert.That(tree.Nested.Contains(childOne));
		}
		[Test]
		public void Subtree()
		{
			var pb1 = new Body();
			var pb2 = new Body();
			var pb3 = new Body();
			var collection2 = new Body { pb2, pb3 };
			var root = new Body { pb1, collection2 };
			var actual = root.GetSubtreeChildrenFirst().ToList();
			Assert.AreEqual(5, actual.Count);
			CheckValidSubtree(actual);
			Assert.That(new List<Body> { pb1 },
				Is.EquivalentTo(pb1.GetSubtreeChildrenFirst().ToList()));

			actual = collection2.GetSubtreeChildrenFirst().ToList();
			Assert.AreEqual(3, actual.Count);
			CheckValidSubtree(actual);

			var c1 = new Body();
			var c2 = new Body();
			var c3 = new Body();
			c1.Add(c2);
			c2.Add(c3);
			actual = c1.GetSubtreeChildrenFirst().ToList();
			Assert.AreEqual(3, actual.Count);
			CheckValidSubtree(actual);
		}

		private static void CheckValidSubtree(IList<Body> actual)
		{
			foreach (var body in actual.Where(x => x.Parent != null))
			{
				var index = actual.IndexOf(body);
				var parentIndex = actual.IndexOf(body.Parent);
				if (parentIndex > 0)
					Assert.Less(index, parentIndex);
			}
		}
	}
}
