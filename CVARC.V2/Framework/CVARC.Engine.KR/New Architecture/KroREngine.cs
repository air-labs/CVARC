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

    public class KroREngine : IPassiveEngine
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

		PhysicalManager physicalManager;

        public void Initialize(IWorld world)
        {
            World = world;
            Root = new Body();
            Root.ChildAdded += Root_ChildAdded;
            DrawerFactory = new DrawerFactory(Root);
			physicalManager = new PhysicalManager();
			physicalManager.InitializeEngine(PhysicalEngines.Farseer, Root);
            Logger = new ReplayLogger(Root, 0.1);
            World.Exit += World_Exit;
        }

        void World_Exit()
        {
            if (World.Configuration.Settings.LegacyLogFile == null) return;
            var engine = World.Engine as KroREngine;
            var replay = engine.GetReplay();
            var finalScores = World.Scores.GetAllScores().Select(z => z.Item2.ToString()).Aggregate((a, b) => a + ":" + b);
            var time = World.Clocks.CurrentTime;

            File.WriteAllLines(World.Configuration.Settings.LegacyLogFile,
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

 
        public void Update(double lastTime, double thisTime)
        {
            var dt = thisTime-lastTime;

            

            while (dt > 1e-5)
            {
                Logger.LogBodies();
                foreach (var e in RequestedSpeeds)
                    GetBody(e.Key).Velocity = e.Value;
                physicalManager.MakeIteration(Math.Min(InternalDeltaTime,dt), Root);
                foreach (Body body in Root)
                    body.Update(dt);
                dt -= InternalDeltaTime;
                if (!World.Configuration.Settings.SpeedUp)
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

        public Body GetBodyOrException(string name)
        {
            var body = GetBody(name);
            if (body == null) throw new Exception("The body with id '" + name + "' was not found in the world");
            return body;
        }

        public Frame3D GetAbsoluteLocation(string id)
        {
            var body = GetBodyOrException(id);
            return body.GetAbsoluteLocation();
        }


        public void DefineCamera(string cameraName, string host, RobotCameraSettings settings)
        {
            //var hostBody = GetBodyOrException(host);
            //Cameras[cameraName] = new CVARCEngineCamera(hostBody, DrawerFactory, settings);
        }

        public byte[] GetImageFromCamera(string cameraName)
        {
            return new byte[0];
            //return Cameras[cameraName].Measure();
        }

        public string GetReplay()
        {
            return ConverterToJavaScript.Convert(Logger.SerializationRoot);
        }


        public void DefineKinect(string kinectName, string host)
        {
            Kinects[kinectName] = new Kinect(GetBodyOrException(host));
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
        private readonly Dictionary<int, Density> densityById = new Dictionary<int, Density>();

        public void Attach(string _objectToAttach, string _host, Frame3D relativePosition)
        {
            var objectToAttach = GetBodyOrException(_objectToAttach);
            var host = GetBodyOrException(_host);
           
            if (objectToAttach.Parent != Root)
                throw new Exception("Object '" + _objectToAttach + "' is already attached");
            Root.Remove(objectToAttach);
            objectToAttach.Location = relativePosition;
            frictionCoefficientsById.SafeAdd(objectToAttach.Id, objectToAttach.FrictionCoefficient);
            densityById.SafeAdd(objectToAttach.Id, objectToAttach.Density);
            objectToAttach.FrictionCoefficient = 0;
            objectToAttach.Density = Density.None;
            
            host.Add(objectToAttach);
        }

        public void Detach(string _objectToDetach, Frame3D absolutePosition)
        {
            var objectToDetach = GetBodyOrException(_objectToDetach);
            var host = objectToDetach.Parent;
            if (host == Root)
                throw new Exception("Cannot detach '" + _objectToDetach + "' - it is not attached");
            host.Remove(objectToDetach);

            objectToDetach.FrictionCoefficient = frictionCoefficientsById.SafeGet(objectToDetach.Id);
            objectToDetach.Density = densityById.SafeGet(objectToDetach.Id);
            objectToDetach.Location = absolutePosition;
            objectToDetach.Velocity = new Frame3D(0, 0, 0);
            Root.Add(objectToDetach);
        }

        public string FindParent(string objectId)
        {
            var obj = GetBodyOrException(objectId);
            var parent = obj.Parent;
            if (parent == Root) return null;
            return parent.NewId;
        }

        public void DeleteObject(string objectId)
        {
            var obj = GetBodyOrException(objectId);
            obj.Parent.Remove(obj);
        }
    }
}
