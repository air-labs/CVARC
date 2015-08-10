using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
	public class DWMWorldState : DemoWorldState
	{
		//Distortion in control, i.e. wheels velocities multiplier
		public double ControlDistortion = 1;

		//Coefficient of distortion in Accelerometer
		public double AccelerometerDistortion = 0;

		public DWMWorldState() : this(0, 0) { }

		public DWMWorldState(double controlDistortion, double accelerometerDistortion)
		{
			Robots = new List<DemoRobotData>
				{
					new DemoRobotData
					{
						 Color= ObjectColor.Red,
						 IsRound=true,
						 RobotName=TwoPlayersId.Left,
						 X=0,
						 Y=0,
						 YSize=5,
						 XSize=5,
						 ZSize=10
					}
				};
			this.ControlDistortion = controlDistortion;
			this.AccelerometerDistortion = accelerometerDistortion;
		}
	}
}
