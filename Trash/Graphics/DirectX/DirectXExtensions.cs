using System;
using System.Drawing;
using AIRLab.Mathematics;
using CVARC.Core;
using SlimDX;
using SlimDX.Direct3D9;
using Matrix = SlimDX.Matrix;

namespace CVARC.Graphics.DirectX
{
	public static class DirectXExtensions
	{
		public static ExtendedMaterial GetDirectXMaterial(this Color color)
		{
			return new ExtendedMaterial
			       	{
			       		MaterialD3D = new Material
			       		              	{
			       		              		Ambient = color,
			       		              		Diffuse = color,
			       		              		Specular = color,
			       		              		Power = 15f
			       		              	}
			       	};
		}

		/// <summary>
		/// Возвращает матрицу координат тела относительно родителя
		/// (в координатной системе DirectX)
		/// </summary>
		/// <param name="body">Тело</param>
		/// <returns></returns>
		public static Matrix GetRelativeLocationMatrix(this Body body)
		{
			return body.Location.ToDirectXMatrix();
		}

		/// <summary>
		/// Возвращает матрицу абсолютных координат тела
		/// (в коорлинатной системе DirectX)
		/// </summary>
		/// <param name="body">Тело</param>
		/// <returns></returns>
		public static Matrix GetAbsoluteLocationMatrix(this Body body)
		{
			return body.GetAbsoluteLocation().ToDirectXMatrix();
		}

		/// <summary>
		/// Возвращает матрицу в системе координат DirectX
		/// </summary>
		/// <param name="frame">Фрейм</param>
		/// <returns></returns>
		public static Matrix ToDirectXMatrix(this Frame3D frame)
		{
			//Если нет кватерниона - поворачиваем исходя из Pitch, yaw, roll - иначе через кватернион.
			if (!frame.Orientation.HasValue)
			{
				return Matrix.RotationYawPitchRoll((float) frame.Pitch.Radian,
				                                   (float) frame.Roll.Radian, -(float) frame.Yaw.Radian)*
				       Matrix.Translation(-(float) frame.X, (float) frame.Y, (float) frame.Z);
			}
			Quat quat = frame.Orientation.Value;
			var q = new Quaternion((float) quat.X, (float) quat.Y, (float) quat.Z, (float) quat.W);
			return Matrix.RotationQuaternion(q)*
			       Matrix.Translation(-(float) frame.X, (float) frame.Y, (float) frame.Z);
		}

		/// <summary>
		/// Из точки возвращает вектор для DirectX
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public static Vector3 ToDirectXVector(this Point3D point)
		{
			return new Vector3(-(float) point.X, (float) point.Y, (float) point.Z);
		}

		///<summary>
		/// Создает DirectX-овский объект Light из данных настроек.
		/// </summary>
		/// <returns></returns>
		public static Light ToDirectXLight(this LightSettings lsets)
		{
			Color color = Color.FromName(lsets.ColorString);
			var l = new Light
			        {
			        	Diffuse = color,
			        	Specular = color,
			        	Direction = lsets.Direction.ToDirectXVector(),
						Position = lsets.Position.ToDirectXVector(),
			        	Type = LightType.Directional,
						Phi = (float) lsets.OuterAngle.Radian,
						Theta = (float) lsets.InnerAngle.Radian,
						Falloff = 1.0f,
						Range = 1000,
						Attenuation0 =1f,
 						Attenuation1 = 0.001f,
			        };
			if (lsets.Type == LightSettings.MyLightType.Ambient)
			{
				l.Ambient = color;
				l.Direction = -Vector3.UnitZ;
			}

			LightType x;
			if (Enum.TryParse(lsets.Type.ToString(), out x))
				l.Type = x;
			return l;
		}

		public static Vector3 ToDirectXVector(this Frame3D frame)
		{
			return new Vector3(-(float) frame.X, (float) frame.Y, (float) frame.Z);
		}
	}
}