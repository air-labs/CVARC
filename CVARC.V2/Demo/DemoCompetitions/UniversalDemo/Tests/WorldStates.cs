using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
	public class KnownWorldStates
	{
		public static  DemoWorldState EmptyWithOneRobot(bool robotIsRectangular)
		{
			return new DemoWorldState
			{
				Robots = 
				{
					new DemoRobotData
					{
						 Color= ObjectColor.Red,
						 IsRound=!robotIsRectangular,
						  RobotName=TwoPlayersId.Left,
						   X=0,
						   Y=0,
						   YSize=10,
						   XSize=10,
						   ZSize=30
					}
				}
			};
		}
	}
}
