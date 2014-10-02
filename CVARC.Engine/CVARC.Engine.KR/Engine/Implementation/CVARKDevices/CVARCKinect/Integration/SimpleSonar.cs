/*
 * 
 * I Think we never used this class

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Basic.Sensors;
using CVARC.Core;
using CVARC.Graphics;
using NUnit.Framework;

namespace kinect.Integration
{
	public class SimpleSonar: Sensor<SonarData>
	{
	    public SimpleSonar(Robot robot, World world, DrawerFactory factory) 
            : base(robot, world, factory)
	    {
            _worldRoot = robot.Body.TreeRoot;
            _settings = new SimpleSonarSettings(robot.Body.GetAbsoluteLocation(), Angle.FromGrad(90), 400);
	    }

        public override SonarData Measure()
		{
		    var result = new List<double>();
		    var angle = -_settings.ViewAngle/2.0;
            for (int i = 0; i < _settings.Resolution; i++)
            {
                Ray ray = new Ray(_settings.Location.ToPoint3D(), SensorRotation.HorisontalRotation(_settings.Location, angle));
                //здесь мы перебираем все объекты, которые есть в мире
                var dist = double.PositiveInfinity;
                foreach (var body in _worldRoot.GetSubtreeChildrenFirst())
                    dist = Math.Min(dist, Intersector.Intersect(body, ray));
                result.Add(dist);
                angle += _settings.Step;
            }
                
			return new SonarData(result.ToArray());
		}
        
		private Body _worldRoot;
		private SimpleSonarSettings _settings;
	}

    [DataContract]
    public class SonarData : ISensorData
    {
        [DataMember]
        public double[] Data { get; set; }

        public SonarData(double[] result)
        {
            Data = result;
        }
    }

    internal class SimpleSonarSettings
	{
        public Frame3D Location { get; private set; }
        public Angle ViewAngle { get; private set; }
        public int Resolution { get; private set; }
        public Angle Step { get; private set; }

        public SimpleSonarSettings(Frame3D location, Angle viewAngle, int resolution)
        {
            Location = location;
            ViewAngle = viewAngle;
            Resolution = resolution;
            Step = ViewAngle/Resolution;
        }
	}
    
    [TestFixture]
    internal class SimpleSonarTest
    {
        //Сонар
        private static readonly Frame3D Location = new Frame3D(-2, 0, 0.5);
        private static readonly Angle ViewAngle = Angle.FromRad(Math.PI);
        private const int Resolution = 180;
        //Шар
        private static readonly Frame3D BallPosition = new Frame3D(0, 0, 0);
        private static readonly Ball Ball = new Ball { Location = BallPosition, Radius = 1};
        //Цилиндр
        private static readonly Frame3D CylinderPosition = new Frame3D(0, 0, 0);
        private static readonly Cylinder Cylinder = new Cylinder
            {
                Location = CylinderPosition,
                RBottom = 1.5,
                RTop = 1.5,
                Height = 3
            };
        //Бокс
        private static readonly Frame3D BoxPosition = new Frame3D(0, 0, 0, Angle.FromRad(0), Angle.FromRad(Math.PI/4.0), Angle.FromRad(0));
        private static readonly Box Box = new Box { Location = BoxPosition, XSize = 1, YSize = 1, ZSize = 1 };

        private readonly object[][] _data = new []
            {
                new object[]
                    {
                        new SimpleSonarSettings(Location, ViewAngle, Resolution),
                        Ball
                    },
                    new object[]
                    {
                        new SimpleSonarSettings(Location, ViewAngle, Resolution),
                        Cylinder
                    },
                    new object[]
                    {
                        new SimpleSonarSettings(Location, ViewAngle, Resolution),
                        Box
                    }
            };
    }
}

*/