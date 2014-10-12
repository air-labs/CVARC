using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using AIRLab.Mathematics;
using CVARC.Basic.Sensors;
using CVARC.Core;

namespace kinect.Integration
{
    public class Kinect
    {
        private Body _worldRoot;
        private KinectSettings _settings;
        private Body _robot;

        private Frame3D GetCameraLocation(Body a)
        {
            return new Frame3D(a.Location.X + movementDistance * Math.Cos(a.Location.Yaw.Radian),
                               a.Location.Y + movementDistance * Math.Sin(a.Location.Yaw.Radian),
                               a.Location.Z + 30, a.Location.Pitch.AddGrad(45), a.Location.Yaw,
                               a.Location.Roll);
        }

        private const int movementDistance = 30;
        private const int size = 50;

        public Kinect(Body body)
        {
            _robot = body; //it is so?
            _worldRoot = body.TreeRoot;
            _settings = new KinectSettings(Angle.FromGrad(120), Angle.FromGrad(120 / 1.35), size, (int)(size / 1.35), 200.0);
        }

        public ImageSensorData Measure()
        {
            var tmpLocation = GetCameraLocation(_robot);
            var result = new KinectData(_settings.VerticalResolution, _settings.HorisontalResolution);
            var horisontalAngle = -_settings.HorisontalViewAngle / 2.0;
            var verticalAngle = -_settings.VerticalViewAngle / 2.0;
            for (int i = 0; i < _settings.VerticalResolution; i++)
            {
                Frame3D mediateDirection = SensorRotation.VerticalFrameRotation(tmpLocation, -verticalAngle);
                horisontalAngle = -_settings.HorisontalViewAngle / 2.0;
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
            return new ImageSensorData(result.GetBytes());
        }
    }

    public class KinectData : IImageSensorData
    {
        public double[,] Depth;

        public KinectData(int i, int j)
        {
            Depth = new double[i, j];
        }

        public byte[] GetBytes()
        {
            var bmp = GetImage();
            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public Bitmap GetImage()
        {
            //TODO: вернуть FastBitmap?

            //var img = new FastBitmap(Depth.GetLength(1), Depth.GetLength(0));
            //for (int i = 0; i < img.Width; ++i)
            //    for (int j = 0; j < img.Height; ++j)
            //    {
            //        var clr = (int)(Depth[img.Height - j - 1, img.Width - i - 1]);
            //        if (clr > 255) clr = 255;
            //        if (clr < 0) clr = 255;
            //        img[i, j] = Color.FromArgb(clr, clr, clr);
            //    }
            //var bmp = img.ToBitmap();
            //return bmp;

            var img = new Bitmap(Depth.GetLength(1), Depth.GetLength(0));
            for (int i = 0; i < img.Width; ++i)
                for (int j = 0; j < img.Height; ++j)
                {
                    var clr = (int)(Depth[img.Height - j - 1, img.Width - i - 1]);
                    if (clr > 255) clr = 255;
                    if (clr < 0) clr = 255;
                    img.SetPixel(i, j, Color.FromArgb(clr, clr, clr));
                }
            return img;


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
        public KinectSettings(Angle horisontalViewAngle, Angle verticalViewAngle, int horisontalResolution, int verticalResolution, double maxDistance)
        {
            HorisontalViewAngle = horisontalViewAngle;
            VerticalViewAngle = verticalViewAngle;
            HorisontalResolution = horisontalResolution;
            VerticalResolution = verticalResolution;
            HStep = HorisontalViewAngle / HorisontalResolution;
            VStep = VerticalViewAngle / VerticalResolution;
            Exclude = new List<Body>();
            MaxDistance = maxDistance;
        }
    }
}
