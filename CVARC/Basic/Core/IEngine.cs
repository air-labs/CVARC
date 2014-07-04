using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic.Controllers;
using CVARC.Basic.Sensors;

namespace CVARC.Basic
{

    public static class IEngineExtensions
    {
        public static void ProcessCommand(this IEngine engine, string actor, Command cmd)
        {
            var Location = engine.GetAbsoluteLocation(actor);
            var speed = new Frame3D(cmd.Move * Math.Cos(Location.Yaw.Radian),
                               cmd.Move * Math.Sin(Location.Yaw.Radian), 0, Angle.Zero, cmd.Angle,
                               Angle.Zero);
            engine.SetSpeed(actor, speed);
            if (cmd.Action != null)
                engine.PerformAction(actor, cmd.Action);

        }
    }

    /// <summary>
    /// An engine is an entity that performs drawing operations and physical operations on the world
    /// </summary>
    public interface IEngine
    {
        void Initialize(ISceneSettings settings);
        /// <summary>
        /// Speed is specified in absolute coordinates
        /// </summary>
        /// <param name="name"></param>
        /// <param name="speed"></param>
        void SetSpeed(string name, Frame3D speed);
        void PerformAction(string name, string action);
        Frame3D GetAbsoluteLocation(string name);

        void DefineCamera(string cameraName, string host, RobotCameraSettings settings);
        byte[] GetImageFromCamera(string cameraName);

        void DefineKinect(string kinectName, string host);
        ImageSensorData GetImageFromKinect(string kinectName);

        void RunEngine(double timeInSeconds, bool inRealTime);
        string GetReplay();

        IEnumerable<string> GetAllObjects();
    }
}
