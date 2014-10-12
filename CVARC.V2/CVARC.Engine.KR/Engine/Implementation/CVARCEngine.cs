//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using AIRLab.Mathematics;
//using CVARC.Basic.Engine;
//using CVARC.Basic.Sensors;
//using CVARC.Core;
//using CVARC.Core.Replay;
//using CVARC.Graphics;
//using CVARC.Physics;
//using kinect.Integration;

//namespace CVARC.Basic
//{
//    public class CVARCEngine : IEngine, ICvarcEngine
//    {
//        public  DrawerFactory DrawerFactory { get; private set; }
//        Body Root { get; set; }
//        Dictionary<string, Frame3D> RequestedSpeeds = new Dictionary<string, Frame3D>();
//        Dictionary<string, CVARCEngineCamera> Cameras = new Dictionary<string, CVARCEngineCamera>();
//        Dictionary<string, Kinect> Kinects = new Dictionary<string, Kinect>();
//        public ReplayLogger Logger { get; private set; }
//        public ICvarcRules Rules { get; internal set; }

//        public CVARCEngine(ICvarcRules rules)
//        {
//            Rules = rules;
//        }

//        public void SetSpeed(string id, Frame3D velocity)
//        {
//            RequestedSpeeds[id] = velocity;
//        }

//        public Frame3D GetSpeed(string id)
//        {
//            return RequestedSpeeds[id];
//        }

//        public void Initialize(ISceneSettings settings)
//        {
//            Root = Rules.CreateWorld(this, settings);
//            DrawerFactory = new DrawerFactory(Root);
//            PhysicalManager.InitializeEngine(PhysicalEngines.Farseer, Root);
//            Logger = new ReplayLogger(Root, 0.1);
//        }

//        public Body GetBody(string name)
//        {
//            return Root.First(z => z.Id.ToString() == name);
//        }

//        public Frame3D GetAbsoluteLocation(string id)
//        {
//            return GetBody(id).Location;
//        }

//        public void DefineCamera(string cameraName, string host, RobotCameraSettings settings)
//        {
//            Cameras[cameraName] = new CVARCEngineCamera(GetBody(host), DrawerFactory, settings);
//        }

//        public byte[] GetImageFromCamera(string cameraName)
//        {
//            return Cameras[cameraName].Measure();
//        }

//        public void RunEngine(double timeInSeconds, bool inRealTime)
//        {
//            double dt = 1.0 / 100;
//            int span = (int)(dt * 1000);
//            for (double t = 0; t < timeInSeconds; t += dt)
//            {
//                Logger.LogBodies();

//                var speeds = RequestedSpeeds.ToArray();
//                foreach (var e in speeds)
//                    GetBody(e.Key).Velocity = e.Value;

//                PhysicalManager.MakeIteration(dt, Root);
//                foreach (Body body in Root)
//                    body.Update(1 / 60);
//                if (inRealTime)
//                    Thread.Sleep(span);
//            }
//        }

//        public string GetReplay()
//        {
//            return ConverterToJavaScript.Convert(Logger.SerializationRoot);
//        }

//        public IEnumerable<IGameObject> GetChilds(string id)
//        {
//            return Root.Single(z => z.Id.ToString() == id).Nested.Select(GetGameObject);
//        }

//        public event OnCollisionEventHandler OnCollision;
//        public void RaiseOnCollision(string firstBodyId, string secondBodyId, CollisionType collisionType)
//        {
//            if (OnCollision != null)
//                OnCollision(new OnCollisionEventHandlerArgs(firstBodyId, secondBodyId, collisionType));
//        }

//        public void DefineKinect(string kinectName, string host)
//        {
//            Kinects[kinectName] = new Kinect(GetBody(host));
//        }

//        private IGameObject GetGameObject(Body body)
//        {
//            return new GameObject(body.Id.ToString(), body.Type);
//        }

//        public ImageSensorData GetImageFromKinect(string kinectName)
//        {
//            return Kinects[kinectName].Measure();
//        }

//        public IEnumerable<IGameObject> GetAllObjects()
//        {
//            return Root.Select(GetGameObject);
//        }

//        public void PerformAction(string id, string action)
//        {
//            Rules.PerformAction(this, id, action);
//        }
//    }
//}
