using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Basic.Sensors;

namespace CVARC.V2
{
    public interface IEngine
    {
        void Initialize(IWorld world);
        void SetSpeed(string id, Frame3D speed);
        Frame3D GetSpeed(string id);
        Frame3D GetAbsoluteLocation(string id);
        bool ContainBody(string id);
        void DefineCamera(string cameraName, string host, RobotCameraSettings settings);
        byte[] GetImageFromCamera(string cameraName);
        void DefineKinect(string kinectName, string host);
        ImageSensorData GetImageFromKinect(string kinectName);

        event Action<string, string> Collision;
        void Attach(string objectToAttach, string host, Frame3D relativePosition);
        void Detach(string objectToDetach, Frame3D absolutePosition);
        string FindParent(string objectId);

        void DeleteObject(string objectId);
    }

    public static class IEngineExtensions
    {
        public static bool IsAttached(this IEngine engine, string objectId)
        {
            return engine.FindParent(objectId) != null;
        }
    }
}
