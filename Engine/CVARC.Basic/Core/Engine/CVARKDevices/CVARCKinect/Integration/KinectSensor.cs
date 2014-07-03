using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic.Sensors;
using CVARC.Basic.Sensors.Core;

namespace CVARC.Basic.Core
{
    class KinectSensor : Sensor<ImageSensorData>
    {
        string KinectName;

        public KinectSensor(Robot robot, World world)
            : base(robot, world)
        {
            KinectName=robot.Name+"Kinect";
            world.Engine.DefineKinect(KinectName, robot.Name);
        }



        public override ImageSensorData Measure()
        {
            return World.Engine.GetImageFromKinect(KinectName);
        }
    }
}
