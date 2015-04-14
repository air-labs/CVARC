﻿using CVARC.V2;
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

		public static DemoWorldState InteractionScene(bool robotIsRectangular)
		{
			var scene = EmptyWithOneRobot(robotIsRectangular);
			scene.Objects.Add(new DemoObjectData
			{
				XSize=80,
				YSize=5,
				ZSize=10,
				X=0,
				Y=30,
				Color= ObjectColor.Black,
				IsStatic=true

			});
			scene.Objects.Add(new DemoObjectData
				{
					XSize=15,
					YSize=15,
					ZSize=15,
					X=50,
					Y=0,
					IsStatic=false,
					Color= ObjectColor.Blue
				});
			return scene;
		}
	}
}