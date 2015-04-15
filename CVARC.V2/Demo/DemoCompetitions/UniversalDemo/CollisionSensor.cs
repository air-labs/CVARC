using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Demo
{
	[DataContract]
	public class CollisionData
	{
		[DataMember]
		public double Time { get; set; }
		[DataMember]
		public CollisionCase CollisionCase { get; set; }

		[DataMember]
		public bool Guilty { get; set; }
	}

	public class CollisionSensor : Sensor<List<CollisionData>, DemoRobot>
	{

		public override List<CollisionData> Measure()
		{
			return Actor.World.CurrentCollisions;
		}
	}
}
