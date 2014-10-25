using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;
using Demo;

namespace Demo.KroR
{
    public class Camera : Competitions
    {
        public Camera()
            : base(new CameraLogicPart(), new KroREnginePart(), new CameraManagerPart())
        { }
    }
}
