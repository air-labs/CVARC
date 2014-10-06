using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AIRLab.Mathematics;
using CVARC.Basic.Engine;
using CVARC.Basic.Sensors;
using CVARC.Core;
using CVARC.Core.Replay;
using CVARC.Graphics;
using CVARC.Physics;
using kinect.Integration;

namespace CVARC.V2
{

    public class KroREngine : IEngine
    {
        const double ExternalDeltaTime = 0.02;
        const double InternalDeltaTime = 0.001;
        public DrawerFactory DrawerFactory { get; private set; }
        public IWorld World { get; private set; }
        public Body Root { get; private set; }
        Dictionary<string, Frame3D> RequestedSpeeds = new Dictionary<string, Frame3D>();
        Dictionary<string, CVARCEngineCamera> Cameras = new Dictionary<string, CVARCEngineCamera>();
        Dictionary<string, Kinect> Kinects = new Dictionary<string, Kinect>();
        HashSet<Body> installedCollisionDetector = new HashSet<Body>();
        public ReplayLogger Logger { get; private set; }

        public void Initialize(IWorld world)
        {
            World = world;
            Root = new Body();
            Root.ChildAdded += Root_ChildAdded;
            DrawerFactory = new DrawerFactory(Root);
            PhysicalManager.InitializeEngine(PhysicalEngines.Farseer, Root);
            Logger = new ReplayLogger(Root, 0.1);
            World.Clocks.AddRenewableTrigger(InternalDeltaTime, Updates);
        }


        void Root_ChildAdded(Body firstObject)
        {
            if (!installedCollisionDetector.Contains(firstObject))
            {
                installedCollisionDetector.Add(firstObject);
                firstObject.ChildAdded += Root_ChildAdded;
                firstObject.Collision += (secondObject) =>
                    {
                        if (firstObject.NewId != null && secondObject.NewId != null)
                            OnCollision(firstObject.NewId, secondObject.NewId);
                    };
            }
        }

 
        void Updates(RenewableTriggerData data, out double nextTime)
        {
            var dt = data.ThisCallTime - data.PreviousCallTime;

            foreach (var e in RequestedSpeeds)
                GetBody(e.Key).Velocity = e.Value;

            while (dt > 1e-5)
            {
                PhysicalManager.MakeIteration(Math.Min(InternalDeltaTime,dt), Root);
                dt -= InternalDeltaTime;
            }
            foreach (Body body in Root)
                body.Update(dt);

            nextTime = data.ThisCallTime + ExternalDeltaTime;
        }

        public void SetSpeed(string id, Frame3D velocity)
        {
            lock (RequestedSpeeds)
            {
                RequestedSpeeds[id] = velocity;
            }
        }

        public Frame3D GetSpeed(string id)
        {
            lock (RequestedSpeeds)
            {
                return RequestedSpeeds[id];
            }
        }

        public Body GetBody(string name)
        {
            return Root.GetSubtreeChildrenFirst().FirstOrDefault(z => z.NewId == name);
        }

        public Frame3D GetAbsoluteLocation(string id)
        {
            var body = GetBody(id);
            if (body == null) throw new Exception("Id not found in Engine");
            return body.Location;
        }


        public void DefineCamera(string cameraName, string host, RobotCameraSettings settings)
        {
            Cameras[cameraName] = new CVARCEngineCamera(GetBody(host), DrawerFactory, settings);
        }

        public byte[] GetImageFromCamera(string cameraName)
        {
            return Cameras[cameraName].Measure();
        }

        public string GetReplay()
        {
            return ConverterToJavaScript.Convert(Logger.SerializationRoot);
        }


        public void DefineKinect(string kinectName, string host)
        {
            Kinects[kinectName] = new Kinect(GetBody(host));
        }

        public event System.Action<string, string> Collision;

        void OnCollision(string firstObject, string secondObject)
        {
            if (Collision!=null)
                Collision(firstObject,secondObject);
        }


        public ImageSensorData GetImageFromKinect(string kinectName)
        {
            return Kinects[kinectName].Measure();
        }


        public bool ContainBody(string id)
        {
            return GetBody(id) != null;
        }
    }
}
