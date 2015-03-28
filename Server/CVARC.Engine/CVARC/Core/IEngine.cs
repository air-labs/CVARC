using System;
using System.Collections.Generic;
using AIRLab.Mathematics;
using CVARC.Basic.Controllers;
using CVARC.Basic.Sensors;

namespace CVARC.Basic
{
    public static class IEngineExtensions
    {
        public static void ProcessCommand(this IEngine engine, string actor, Command cmd)
        {
            if (cmd.Action == CommandAction.WaitForExit)
                cmd.Time = 100000;
            var location = engine.GetAbsoluteLocation(actor);
            var speed = new Frame3D(cmd.LinearVelocity * Math.Cos(location.Yaw.Radian),
                               cmd.LinearVelocity * Math.Sin(location.Yaw.Radian), 0, Angle.Zero, cmd.AngularVelocity,
                               Angle.Zero);
            engine.SetSpeed(actor, speed);
            if (cmd.Action != CommandAction.None)
                engine.PerformAction(actor, cmd.Action.ToString());
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
        /// <param name="id"></param>
        /// <param name="speed"></param>
        void SetSpeed(string id, Frame3D speed);
        Frame3D GetSpeed(string id);
        void PerformAction(string id, string action);
        Frame3D GetAbsoluteLocation(string id);

        void DefineCamera(string cameraName, string host, RobotCameraSettings settings);
        byte[] GetImageFromCamera(string cameraName);

        void DefineKinect(string kinectName, string host);
        ImageSensorData GetImageFromKinect(string kinectName);

        void RunEngine(double timeInSeconds, bool inRealTime);
        string GetReplay();

        IEnumerable<IGameObject> GetChilds(string id);

        event OnCollisionEventHandler OnCollision;
        void RaiseOnCollision(string firstBodyId, string secondBodyId, CollisionType collisionType);

        IEnumerable<IGameObject> GetAllObjects();
    }

    public delegate void OnCollisionEventHandler(OnCollisionEventHandlerArgs args);

    public class OnCollisionEventHandlerArgs
    {
        public OnCollisionEventHandlerArgs(string firstBodyId, string secondBodyId, CollisionType collisionType)
        {
            FirstBodyId = firstBodyId;
            SecondBodyId = secondBodyId;
            CollisionType = collisionType;
        }

        public string FirstBodyId { get; set; }
        public string SecondBodyId { get; set; }
        public CollisionType CollisionType { get; set; }
    }

    public enum CollisionType
    {
        RedWallRepaired,
        GreenWallRepaired,
        BlueWallRepaired,
        RobotCollision
    }

    public interface IGameObject
    {
        string Type { get; }
        string Id { get; }     
    }
}
