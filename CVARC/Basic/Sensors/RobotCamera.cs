using System;
using System.IO;
using AIRLab.Mathematics;
using AIRLab.Thornado;

namespace CVARC.Basic.Sensors
{
	public class RobotCamera : Sensor<ImageSensorData>
	{
        public readonly string CameraName;

	    public RobotCamera(Robot robot) 
            : base(robot)
	    {
            CameraName=robot.Name + "Camera";
            Engine.DefineCamera(CameraName, robot.Name, new RobotCameraSettings());
	    }

	    public RobotCameraSettings Settings { get; private set; }

	    /// <summary>
		/// Снимает изображение с камеры и возвращает объект с данными камеры. 
		/// </summary>
		/// <returns></returns>
        public override ImageSensorData Measure()
	    {
	        return new ImageSensorData(Engine.GetImageFromCamera(CameraName));
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
        public Frame3D Location = new Frame3D(30, 0, 30, Angle.FromGrad(-45), Angle.Zero, Angle.Zero);

		/// <summary>
		/// Писать в файл для дебага
		/// </summary>
		[Thornado]
		public bool WriteToFile;
	}
}