using System;
using System.IO;
using AIRLab.Mathematics;
using System.Drawing;
using CVARC.V2;


namespace CVARC.V2
{
    public abstract class RobotCamera : Sensor<byte[],IActor>
    {
        string CameraName;
        RobotCameraSettings settings;

        protected RobotCamera(RobotCameraSettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Снимает изображение с камеры и возвращает объект с данными камеры. 
        /// </summary>
        /// <returns></returns>
        public override byte[] Measure()
        {
            if (CameraName == null)
            {
                CameraName = Actor.ObjectId + ".Camera";
                Actor.World.Engine.DefineCamera(CameraName, Actor.ObjectId, settings);
            }
            return Actor.World.Engine.GetImageFromCamera(CameraName);
        }


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

    }
	
	
    //[Serializable]
    //public class RobotCameraData : IImageSensorData
    //{
    //    /// <summary>
    //    /// Массив байтов, содержащий изображение в формате jpeg
    //    /// </summary>
    //    public byte[] Bitmap;

    //    public string GetStringRepresentation()
    //    {
    //        return Convert.ToBase64String(Bitmap);
    //    }

    //    public Bitmap GetImage()
    //    {
    //        return (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(new MemoryStream(Bitmap));
    //        //return new Bitmap(FastBitmap.FromBMPStream(new MemoryStream(Bitmap)).ToBitmap());
    //    }
    //}

	[Serializable]
	public class RobotCameraSettings
	{
		/// <summary>
		/// Угол зрения
		/// </summary>

		public Angle ViewAngle=Angle.HalfPi;

		/// <summary>
		/// Точка крепления камеры
		/// </summary>

		public Frame3D Location = new Frame3D(0, 0, 10, Angle.FromGrad(-25), Angle.Zero, Angle.Zero);

		public int ImageWidth = 800;

		public int ImageHeight = 600;
		/// <summary>
		/// Писать в файл для дебага
		/// </summary>

		public bool WriteToFile;

	}
}