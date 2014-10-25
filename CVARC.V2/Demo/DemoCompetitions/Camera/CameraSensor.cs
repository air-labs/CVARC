using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;

namespace Demo
{
    public  class CameraSensor : RobotCamera
    {
        public CameraSensor()
            : base(new RobotCameraSettings { Location = new AIRLab.Mathematics.Frame3D(10, 0, 20, -Angle.Pi / 6, Angle.Zero,Angle.Zero) })
        { }
    }
}
