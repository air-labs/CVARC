using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AIRLab.Mathematics;
using CVARC.Basic.Engine;
using CVARC.Basic.Sensors;
using CVARC.Core;
using CVARC.Core.Replay;
using CVARC.Graphics;
using CVARC.Physics;
using kinect.Integration;

namespace CVARC.Basic
{
    public abstract class CVARCEngine : IEngine
    {
       public  DrawerFactory DrawerFactory { get; private set; }
        Body Root { get; set; }
        //Dictionary<string, Body> Robots = new Dictionary<string, Body>();
        Dictionary<string, Frame3D> RequestedSpeeds = new Dictionary<string, Frame3D>();
        Dictionary<string, CVARCEngineCamera> Cameras = new Dictionary<string, CVARCEngineCamera>();
        Dictionary<string, Kinect> Kinects = new Dictionary<string, Kinect>();
        public ReplayLogger Logger { get; private set; }

        public abstract Body CreateWorld(ISceneSettings settings);
        //public abstract IEnumerable<Body> CreateRobots(Body root, ISceneSettings settings);
        public abstract void PerformAction(string actor, string action);

        public void SetSpeed(string obj, Frame3D velocity)
        {
            RequestedSpeeds[obj] = velocity;
        }

        public void Initialize(ISceneSettings settings)
        {
            Root = CreateWorld(settings);
            //foreach (var e in CreateRobots(Root, settings))
            //    Robots[e.Name] = e;
            DrawerFactory = new DrawerFactory(Root);
            PhysicalManager.InitializeEngine(PhysicalEngines.Farseer, Root);
            Logger = new ReplayLogger(Root, 0.1);
        }

        public Body GetBody(string name)
        {
            return Root.Where(z => z.Name == name).First();
        }

        public Frame3D GetAbsoluteLocation(string name)
        {
            return GetBody(name).Location;
        }


        public void DefineCamera(string cameraName, string host, RobotCameraSettings settings)
        {
            Cameras[cameraName] = new CVARCEngineCamera(GetBody(host), this, settings);
        }

        public byte[] GetImageFromCamera(string cameraName)
        {
            return Cameras[cameraName].Measure();
        }


        public void RunEngine(double timeInSeconds, bool inRealTime)
        {
            double dt = 1.0 / 100;
            int span = (int)(dt * 1000);
            for (double t = 0; t < timeInSeconds; t += dt)
            {
                Logger.LogBodies();

                foreach (var e in RequestedSpeeds)
                    GetBody(e.Key).Velocity = e.Value;

                PhysicalManager.MakeIteration(dt, Root);
                foreach (Body body in Root)
                    body.Update(1 / 60);
                if (inRealTime)
                    Thread.Sleep(span);
            }
        }


        public string GetReplay()
        {
            return ConverterToJavaScript.Convert(Logger.SerializationRoot);
        }


        public void DefineKinect(string kinectName, string host)
        {
            Kinects[kinectName] = new Kinect(GetBody(host));
        }

        public ImageSensorData GetImageFromKinect(string kinectName)
        {
            return Kinects[kinectName].Measure();
        }


        public IEnumerable<string> GetAllObjects()
        {
            return Root.Select(z => z.Name);
        }
    }
}
