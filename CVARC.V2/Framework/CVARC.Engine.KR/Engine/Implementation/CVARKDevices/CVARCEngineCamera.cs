using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic.Sensors;
using CVARC.Core;
using CVARC.Graphics;
using CVARC.Graphics.DirectX;
using CVARC.V2;

namespace CVARC.Basic.Engine
{
    public class CVARCEngineCamera 
    {
        private readonly Body robot;
        public RobotCameraSettings Settings { get; private set; }

        public CVARCEngineCamera(Body body, DrawerFactory factory, RobotCameraSettings settings)
        {
            Settings = settings;
            this.robot = body;
            Angle viewAngle = Settings.ViewAngle;
            _camera = new FirstPersonCamera(this.robot, Settings.Location,
                                            viewAngle, Settings.ImageWidth / (double)settings.ImageHeight);
            _drawer = new OffscreenDirectXDrawer(factory.GetDirectXScene(), settings.ImageWidth,
                                                 settings.ImageHeight);
        }

        

        /// <summary>
        /// Освобождает unmanaged ресурсы, используемые камерой.
        /// </summary>
        public void Dispose()
        {
            _drawer.Dispose();
        }

        /// <summary>
        /// Снимает изображение с камеры и возвращает объект с данными камеры. 
        /// </summary>
        /// <returns></returns>
        public byte[] Measure()
        {
            Settings.Location = robot.GetAbsoluteLocation();
            byte[] data;
            bool result = _drawer.TryGetImage(_camera, out data);
            if (Settings.WriteToFile && result)
                WriteToFile(data);
            return data;
        }

        public const int DefaultHeight = 600;
        public const int DefaultWidth = 800;

        private static void WriteToFile(byte[] bitmap)
        {
            const string tempDir = "CameraTestImages";
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);
            var guid = Guid.NewGuid();
            string path = Path.Combine(tempDir,
                string.Format("test{0}.png", guid));
            File.WriteAllBytes(path, bitmap);
        }

        private OffscreenDirectXDrawer _drawer;
        private FirstPersonCamera _camera;
    }
	
}
