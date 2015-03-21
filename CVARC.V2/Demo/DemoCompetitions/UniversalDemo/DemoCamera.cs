using AIRLab.Mathematics;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
	public class DemoCamera : RobotCamera
	{
		static RobotCameraSettings settings = new RobotCameraSettings 
		{
			 Location=new AIRLab.Mathematics.Frame3D(30,20,0),
			 ViewAngle=Angle.HalfPi,
			  WriteToFile=true,
			  ImageHeight=800,
			  ImageWidth=600

		};

		public DemoCamera()
			: base(settings)
		{ }
	}
}
