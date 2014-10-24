using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using AIRLab.Mathematics;
using NUnit.Framework;

namespace CVARC.Core.Replay
{
	public partial class ObjectLoader
	{
		internal class LoadingBodiesTests
		{
			[Test]
			public void TestLocation()
			{
				var root = new Body();
				var box = new Box();
				root.Add(box);
				var lo = new LoggingObject(box, root);
				Movement movement1 = GetMovement(0, new Frame3D(1, 0, 0));
				lo.Movements.Add(movement1);
				Movement movement2 = GetMovement(300, new Frame3D(0, 1, 0));
				lo.Movements.Add(movement2);
				var ol = new ObjectLoader(lo, new Body());
				Frame3D expectedLoc = Frame3D.Identity;
				for(int i = 0; i <= _expectedLocations.Keys.Max(); i++)
				{
					if(_expectedLocations.ContainsKey(i))
						expectedLoc = _expectedLocations[i];
					ol.LoadLocation(i);
					Assert.AreEqual(expectedLoc, ol._loadedBody.Location);
				}
			}

			[TestCase(true)]
			[TestCase(false)]
			public void TestVisibility(bool startVisible)
			{
				var root = new Body {NewId = "oldRoot"};
				var box = new Body {NewId = "oldBox"};
				if(startVisible)
					root.Add(box);
				LoggingObject lo = SerializeAndDeserialize(new LoggingObject(box, root));
				lo.VisibilityStates.Add(new Visibility(startVisible, 0));
				const int visibilityChangedTime = 13;
				lo.VisibilityStates.Add(new Visibility(!startVisible, visibilityChangedTime));
				var ol = new ObjectLoader(lo, new Body());
				for(int i = 0; i < visibilityChangedTime * 2; i++)
				{
					ol.LoadVisibility(i);
					bool isAttached;
					if(startVisible)
						isAttached = i < visibilityChangedTime;
					else
						isAttached = i >= visibilityChangedTime;
					Assert.AreEqual(isAttached, !lo.Body.TreeRoot.Equals(lo.Body));
				}
			}

			public static T SerializeAndDeserialize<T>(T toSerialize)
			{
				var bf = new BinaryFormatter();
				Stream ms = new MemoryStream();
				bf.Serialize(ms, toSerialize);
				ms.Position = 0;
				return (T)bf.Deserialize(ms);
			}

			private Movement GetMovement(int startTime, Frame3D offset)
			{
				var movement = new Movement(startTime);
				for(int i = 0; i < 100; i++)
				{
					_location = _location.Apply(offset);
					_expectedLocations[startTime + i] = _location;
					movement.SaveLocation(_location);
				}
				return movement;
			}

			private readonly SortedDictionary<double, Frame3D> _expectedLocations = new SortedDictionary<double, Frame3D>();
			private Frame3D _location;
		}
	}
}