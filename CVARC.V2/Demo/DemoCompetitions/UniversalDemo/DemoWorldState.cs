using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Demo
{
	public enum ObjectColor
	{
		Red,
		Blue,
		Green,
		White,
		Black
	}

	[DataContract]
	public class DemoObjectDataBase
	{
		[DataMember]
		public ObjectColor Color { get; set; }
		[DataMember]
		public int X { get; set; }
		[DataMember]
		public int Y { get; set; }
		[DataMember]
		public int XSize { get; set; }
		[DataMember]
		public int YSize { get; set; }
		[DataMember]
		public int ZSize { get; set; }
	}

	[DataContract]
	public class DemoRobotData : DemoObjectDataBase
	{
		[DataMember]
		public string RobotName { get; set; }
		[DataMember]
		public bool IsRound { get; set; }
	}

	public class DemoObjectData : DemoObjectDataBase
	{
		[DataMember]
		public bool IsStatic { get; set; }
	}

	[DataContract]
	public class DemoWorldState : IWorldState
	{
		[DataMember]
		public List<DemoObjectData> Objects { get; set; }
		[DataMember]
		public List<DemoRobotData> Robots { get; set; }

		public DemoWorldState()
		{
			Objects = new List<DemoObjectData>();
			Robots = new List<DemoRobotData>();
		}
	}
}
