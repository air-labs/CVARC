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
using System.IO;

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
            World.Exit += World_Exit;
        }

        void World_Exit()
        {
            if (World.RunMode.Configuration.Settings.LegacyLogFile == null) return;
            var engine = World.Engine as KroREngine;
            var replay = engine.GetReplay();
            var finalScores = World.Scores.GetAllScores().Select(z => z.Item2.ToString()).Aggregate((a, b) => a + ":" + b);
            var time = World.Clocks.CurrentTime;

            File.WriteAllLines(World.RunMode.Configuration.Settings.LegacyLogFile,
                new string[]
                {
                    "Language",
                    finalScores,
                    time.ToString(),
                    replay
                });
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
                Logger.LogBodies();
                foreach (var e in RequestedSpeeds)
                    GetBody(e.Key).Velocity = e.Value;
                PhysicalManager.MakeIteration(Math.Min(InternalDeltaTime,dt), Root);
                foreach (Body body in Root)
                    body.Update(dt);
                dt -= InternalDeltaTime;
                if (!World.RunMode.Configuration.Settings.SpeedUp)
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
                if (RequestedSpeeds.ContainsKey(id))
                    return RequestedSpeeds[id];
                return new Frame3D();
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
            var hostBody = GetBody(host);
            Cameras[cameraName] = new CVARCEngineCamera(hostBody, DrawerFactory, settings);
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


        private readonly Dictionary<int, double> frictionCoefficientsById = new Dictionary<int, double>();


        public void Attach(string _objectToAttach, string _host, Frame3D relativePosition)
        {
            var objectToAttach = GetBody(_objectToAttach);
            if (objectToAttach == null)
                throw new Exception("ObjectToAttach was not found");
            var host = GetBody(_host);
            if (host==null)
                throw new Exception("Host was not found");

            if (objectToAttach.Parent != null)
                throw new Exception("Object '" + _objectToAttach + "' is already attached");
            objectToAttach.Location = relativePosition;
            frictionCoefficientsById.SafeAdd(objectToAttach.Id, objectToAttach.FrictionCoefficient);
            objectToAttach.FrictionCoefficient = 0;
            host.Add(objectToAttach);
        }

        public void Detach(string _objectToDetach, Frame3D absolutePosition)
        {
            var objectToDetach = GetBody(_objectToDetach);
            var host = objectToDetach.Parent;
            if (host == null)
                throw new Exception("Cannot detach '" + _objectToDetach + "' - it is not attached");
            host.Remove(objectToDetach);
            objectToDetach.FrictionCoefficient = frictionCoefficientsById.SafeGet(objectToDetach.Id);
            objectToDetach.Location = absolutePosition;
            objectToDetach.Velocity = new Frame3D(0, 0, 0);
            Root.Add(objectToDetach);
        }

        public string FindParent(string objectId)
        {
            var obj = GetBody(objectId);
            if (obj == null)
                throw new Exception("The object '" + obj + "'was not found");
            var parent = obj.Parent;
            if (parent == null) return null;
            return parent.NewId;
        }
    }
}
