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
        const double InternalDeltaTime = 0.01;
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

 
        internal void Updates(double lastTime, double thisTime)
        {
            var dt = thisTime-lastTime;

            

            while (dt > 1e-5)
            {
                foreach (var e in RequestedSpeeds)
                    GetBody(e.Key).Velocity = e.Value;
                PhysicalManager.MakeIteration(Math.Min(InternalDeltaTime,dt), Root);
                foreach (Body body in Root)
                    body.Update(dt);
                dt -= InternalDeltaTime;
                if (!World.RunMode.Configuration.SpeedUp)
                    Thread.Sleep((int)(InternalDeltaTime * 1000));
            }
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
