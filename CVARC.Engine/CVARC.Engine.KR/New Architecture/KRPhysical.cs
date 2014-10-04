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

    public class KRPhysical : IEngine
    {
        public DrawerFactory DrawerFactory { get; private set; }
        public IWorld World { get; private set; }
        public Body Root { get; private set; }
        Dictionary<string, Frame3D> RequestedSpeeds = new Dictionary<string, Frame3D>();
        Dictionary<string, CVARCEngineCamera> Cameras = new Dictionary<string, CVARCEngineCamera>();
        Dictionary<string, Kinect> Kinects = new Dictionary<string, Kinect>();
        public ReplayLogger Logger { get; private set; }

        public void Initialize(IWorld world)
        {
            World = world;
            Root = (World.Manager as IKRWorldManager).Root;
            DrawerFactory = new DrawerFactory(Root);
            PhysicalManager.InitializeEngine(PhysicalEngines.Farseer, Root);
            Logger = new ReplayLogger(Root, 0.1);
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

        double oldTime = 0;
        public void Tick(double time)
        {
            foreach (var e in RequestedSpeeds)
                GetBody(e.Key).Velocity = e.Value;
            
            double dt = time - oldTime;
            PhysicalManager.MakeIteration(dt, Root);
            foreach (Body body in Root)
                body.Update(dt);
            oldTime = time;
        }

        public Body GetBody(string name)
        {
            return Root.First(z => z.NewId == name);
        }

        public Frame3D GetAbsoluteLocation(string id)
        {
            return GetBody(id).Location;
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



        public ImageSensorData GetImageFromKinect(string kinectName)
        {
            return Kinects[kinectName].Measure();
        }
    }
}
