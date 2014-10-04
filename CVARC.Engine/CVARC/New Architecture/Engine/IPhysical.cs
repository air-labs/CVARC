using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Basic.Sensors;

namespace CVARC.V2
{
    public interface IPhysical
    {
        void Initialize(IWorld world);

        void Tick(double time);
        void SetSpeed(string id, Frame3D speed);
        Frame3D GetSpeed(string id);
        Frame3D GetAbsoluteLocation(string id);
        void DefineCamera(string cameraName, string host, RobotCameraSettings settings);
        byte[] GetImageFromCamera(string cameraName);
        void DefineKinect(string kinectName, string host);
        ImageSensorData GetImageFromKinect(string kinectName);
        event Action<string, string> Collision;
    }
}
