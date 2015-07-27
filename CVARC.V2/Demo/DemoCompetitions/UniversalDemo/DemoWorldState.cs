using AIRLab.Mathematics;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Demo
{
	[Serializable]
	public enum ObjectColor
	{
		Red,
		Blue,
		Green,
		White,
		Black
	}

	[Serializable]
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

	[Serializable]
	[DataContract]
	public class DemoRobotData : DemoObjectDataBase
	{
		[DataMember]
		public Angle Yaw { get; set; }
		[DataMember]
		public string RobotName { get; set; }
		[DataMember]
		public bool IsRound { get; set; }

	}

	[Serializable]
	[DataContract]
	public class DemoObjectData : DemoObjectDataBase
	{
		[DataMember]
		public bool IsStatic { get; set; }
	}

	[Serializable]
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
