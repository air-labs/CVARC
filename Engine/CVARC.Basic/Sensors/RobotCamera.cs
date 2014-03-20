using System;
using System.Drawing;
using System.IO;
using AIRLab.Drawing;
using AIRLab.Mathematics;
using AIRLab.Thornado;
using CVARC.Core;
using CVARC.Graphics;
using CVARC.Graphics.DirectX;

namespace CVARC.Basic.Sensors
{
	public class RobotCamera : Sensor<ImageSensorData>, IDisposable
	{
	    private readonly Body robot;

	    public RobotCamera(Robot robot, World world, DrawerFactory factory) 
            : base(robot, world, factory)
	    {
            Settings = new RobotCameraSettings();
            this.robot = robot.Body;
            Angle viewAngle = Settings.ViewAngle;
            _camera = new FirstPersonCamera(this.robot, Settings.Location,
                                            viewAngle, DefaultWidth / (double)DefaultHeight);
            _drawer = new OffscreenDirectXDrawer(factory.GetDirectXScene(), DefaultWidth,
                                                 DefaultHeight);
	    }

	    public RobotCameraSettings Settings { get; private set; }

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
        public override ImageSensorData Measure()
		{
		    Settings.Location = robot.GetAbsoluteLocation();
			var data = new RobotCameraData();
			bool result = _drawer.TryGetImage(_camera, out data.Bitmap);
			if (Settings.WriteToFile && result)
				WriteToFile(data.Bitmap);
			return new ImageSensorData
			    {
			        Base64Picture = data.GetStringRepresentation()
			    };
		}

		public const int DefaultHeight = 600;
		public const int DefaultWidth = 800;

		private static void WriteToFile(byte[] bitmap)
		{
			const string tempDir = "CameraTestImages";
			if(!Directory.Exists(tempDir))
				Directory.CreateDirectory(tempDir);
			var guid = Guid.NewGuid();
			string path = Path.Combine(tempDir,
				string.Format("test{0}.png", guid));
			File.WriteAllBytes(path, bitmap);
		}

		private OffscreenDirectXDrawer _drawer;
		private FirstPersonCamera _camera;
	}
	
	[Serializable]
	public class RobotCameraData : IImageSensorData
	{
		/// <summary>
		/// Массив байтов, содержащий изображение в формате jpeg
		/// </summary>
		public byte[] Bitmap;

	    public string GetStringRepresentation()
	    {
            return Convert.ToBase64String(Bitmap);
	    }

	    public Bitmap GetImage()
	    {
	        return new Bitmap(FastBitmap.FromBMPStream(new MemoryStream(Bitmap)).ToBitmap());
	    }
	}

	[Serializable]
	public class RobotCameraSettings
	{
		/// <summary>
		/// Угол зрения
		/// </summary>
		[Thornado]
		public Angle ViewAngle=Angle.HalfPi;

		/// <summary>
		/// Точка крепления камеры
		/// </summary>
		[Thornado]
		public Frame3D Location = new Frame3D(0, 0, 10, Angle.FromGrad(-25), Angle.Zero, Angle.Zero);

		/// <summary>
		/// Писать в файл для дебага
		/// </summary>
		[Thornado]
		public bool WriteToFile;
	}
}