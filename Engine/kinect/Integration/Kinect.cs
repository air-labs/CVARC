using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using AIRLab.Drawing;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Basic.Sensors;
using CVARC.Core;
using CVARC.Graphics;

namespace kinect.Integration
{
    public class Kinect : ISensor<ImageSensorData>
    {
        private Body _worldRoot;
        private KinectSettings _settings;
        private Robot _robot;

        private Frame3D GetCameraLocation(Robot a)
        {
            return new Frame3D(a.Body.Location.X + movementDistance*Math.Cos(a.Body.Location.Yaw.Radian),
                               a.Body.Location.Y + movementDistance*Math.Sin(a.Body.Location.Yaw.Radian),
                               a.Body.Location.Z + 30, a.Body.Location.Pitch.AddGrad(45), a.Body.Location.Yaw,
                               a.Body.Location.Roll);
        }

        private const int movementDistance = 30;
        private const int size = 50;

        public void Init(Robot robot, World wrld, DrawerFactory factory)
        {
            _worldRoot = robot.Body.TreeRoot;
            _robot = robot;
            _settings = new KinectSettings(Angle.FromGrad(120), Angle.FromGrad(120/1.35), size, (int) (size/1.35), 200.0);
        }

        public ImageSensorData Measure()
        {
            var tmpLocation = GetCameraLocation(_robot);
            var result = new KinectData(_settings.VerticalResolution, _settings.HorisontalResolution);
            var horisontalAngle = -_settings.HorisontalViewAngle/2.0;
            var verticalAngle = -_settings.VerticalViewAngle/2.0;
            for (int i = 0; i < _settings.VerticalResolution; i++)
            {
                Frame3D mediateDirection = SensorRotation.VerticalFrameRotation(tmpLocation, -verticalAngle);
                horisontalAngle = -_settings.HorisontalViewAngle/2.0;
                for (int j = 0; j < _settings.HorisontalResolution; j++)
                {
                    Frame3D direction = SensorRotation.HorisontalFrameRotation(mediateDirection, horisontalAngle);
                    Ray ray = new Ray(tmpLocation.ToPoint3D(), SensorRotation.GetFrontDirection(direction));
                    var dist = double.PositiveInfinity;
                    foreach (var body in _worldRoot.GetSubtreeChildrenFirst())
                        if (_settings.Exclude.All(a => a != body))
                        {
                            var inter = Intersector.Intersect(body, ray);
                            dist = Math.Min(dist, inter);
                        }
                    result.Depth[i, j] = dist;

                    //verticalAngle += _settings.VStep;
                    horisontalAngle += _settings.HStep;
                }
                verticalAngle += _settings.VStep;
            }
            return new ImageSensorData
                {
                    Base64Picture = result.GetStringRepresentation()
                };
        }
    }

    public class KinectData : IImageSensorData
	{
	    public double[,] Depth;

        public KinectData(int i, int j)
        {
            Depth = new double[i,j];
        }

        public string GetStringRepresentation()
        {
            var bmp = GetImage();
            string base64;
            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                base64 = Convert.ToBase64String(ms.ToArray());
            }
            return base64;
        }

        public Bitmap GetImage()
        {
            var img = new FastBitmap(Depth.GetLength(1), Depth.GetLength(0));
            for (int i = 0; i < img.Width; ++i)
                for (int j = 0; j < img.Height; ++j)
                {
                    var clr = (int)(Depth[img.Height - j - 1, img.Width - i - 1]);
                    if (clr > 255) clr = 255;
                    if (clr < 0) clr = 255;
                    img[i, j] = Color.FromArgb(clr, clr, clr);
                }
            var bmp = img.ToBitmap();
            return bmp;
        }
	}

    public class KinectSettings
	{
        public Angle HorisontalViewAngle { get; private set; }
        public Angle VerticalViewAngle { get; private set; }
        public int HorisontalResolution { get; private set; }
        public int VerticalResolution { get; private set; }
        public Angle HStep { get; private set; }
        public Angle VStep { get; private set; }
        public List<Body> Exclude { get; set; }
        public double MaxDistance { get; private set; }
        public KinectSettings(Angle horisontalViewAngle, Angle verticalViewAngle,  int horisontalResolution, int verticalResolution, double maxDistance)
        {
            HorisontalViewAngle = horisontalViewAngle;
            VerticalViewAngle = verticalViewAngle;
            HorisontalResolution = horisontalResolution;
            VerticalResolution = verticalResolution;
            HStep = HorisontalViewAngle/HorisontalResolution;
            VStep = VerticalViewAngle/VerticalResolution;
            Exclude = new List<Body>();
            MaxDistance = maxDistance;
        }
	}
}
