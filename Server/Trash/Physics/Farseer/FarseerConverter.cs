using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using AIRLab.Mathematics;
using FBody = FarseerPhysics.Dynamics.Body;

namespace CVARC.Core.Physics.FarseerWrap
{
	static public class FarseerConverter
	{
		private static float _displayUnitsToSimUnitsRatio = 10f;
		private static float _simUnitsToDisplayUnitsRatio = 1 / _displayUnitsToSimUnitsRatio;

		#region Frame convertions

		static public Frame3D Vector2ToFrame3D(Vector2 vector)
		{
			return Vector2ToFrame3D(vector, Angle.FromRad(0));
		}

		static public Frame3D Vector2ToFrame3D(Vector2 vector, Angle yaw)
		{
			return Vector2ToFrame3D(vector, yaw, 0);
		}

		static public Frame3D Vector2ToFrame3D(Vector2 vector, Angle yaw, double z)
		{
			return new Frame3D(vector.X, vector.Y, z, Angle.FromRad(0), yaw, Angle.FromRad(0));
		}

		/// <summary>
		/// Вернёт Frame3d c x, y и Yaw из vector и z, pitch и roll из frame.
		/// </summary>
		static public Frame3D Vector2ToFrame3D(Vector2 vector, Angle yaw, Frame3D frame)
		{
			return new Frame3D(vector.X, vector.Y, frame.Z, frame.Pitch, yaw, frame.Roll);
		}

		static public Vector2 Frame3DToVector2(Frame3D frame)
		{
			return new Vector2((float)frame.X, (float)frame.Y);
		}


		/// <summary>
		/// Поставит тело из движка место в нужную позицию и повернёт, проведя конвертирование координат в мир фарсира.
		/// </summary>		
		static public void PutInFrame(FBody fb, Frame3D frame)
		{
			fb.Position = ToSimUnits(Frame3DToVector2(frame));
			fb.Rotation = (float)frame.Yaw.Radian;
		}

		///// <summary>
		///// Запишет коорднаты тела в фрейм, проведя конвертирование координат из мира фарсира.
		///// </summary>		
		//static public Frame3D GetFrame(FarseeBody fb, double z = 0)
		//{
		//    Frame3D frame = Vector2ToFrame3D(ConvertUnits.ToDisplayUnits(fb.Position), Angle.FromRad(fb.Rotation), z);
		//    return frame;
		//}

		/// <summary>
		/// Запишет коорднаты тела в фрейм, проведя конвертирование координат из мира фарсира.
		/// Возьмёт Z, pitch и roll из frame.
		/// </summary>		
		static public Frame3D GetFrame(FBody fb, Frame3D frame)
		{
			Frame3D f = Vector2ToFrame3D(ToDisplayUnits(fb.Position), Angle.FromRad(fb.Rotation), frame);
			return f;
		}

		/// <summary>
		/// Вернёт frame, в котором повёрнуты координаты X и Y и Yaw равен переданному углу.
		/// </summary>		
		static public Frame3D Rotate2D(Frame3D frame, Angle angle)
		{
			double x = frame.X * Math.Cos(angle.Radian) - frame.Y * Math.Sin(angle.Radian);
			double y = frame.X * Math.Sin(angle.Radian) + frame.Y * Math.Cos(angle.Radian);

			return new Frame3D(x, y, frame.Z, Angle.FromGrad(0), angle, Angle.FromGrad(0));
		}

		#endregion

		public static void SetDisplayUnitToSimUnitRatio(float displayUnitsPerSimUnit)
		{
			_displayUnitsToSimUnitsRatio = displayUnitsPerSimUnit;
			_simUnitsToDisplayUnitsRatio = 1 / displayUnitsPerSimUnit;
		}

		public static float ToDisplayUnits(float simUnits)
		{
			return simUnits * _displayUnitsToSimUnitsRatio;
		}

		public static float ToDisplayUnits(int simUnits)
		{
			return simUnits * _displayUnitsToSimUnitsRatio;
		}

		public static Vector2 ToDisplayUnits(Vector2 simUnits)
		{
			return simUnits * _displayUnitsToSimUnitsRatio;
		}

		public static void ToDisplayUnits(ref Vector2 simUnits, out Vector2 displayUnits)
		{
			Vector2.Multiply(ref simUnits, _displayUnitsToSimUnitsRatio, out displayUnits);
		}

		public static Vector3 ToDisplayUnits(Vector3 simUnits)
		{
			return simUnits * _displayUnitsToSimUnitsRatio;
		}

		public static Vector2 ToDisplayUnits(float x, float y)
		{
			return new Vector2(x, y) * _displayUnitsToSimUnitsRatio;
		}

		public static void ToDisplayUnits(float x, float y, out Vector2 displayUnits)
		{
			displayUnits = Vector2.Zero;
			displayUnits.X = x * _displayUnitsToSimUnitsRatio;
			displayUnits.Y = y * _displayUnitsToSimUnitsRatio;
		}

		public static float ToSimUnits(float displayUnits)
		{
			return displayUnits * _simUnitsToDisplayUnitsRatio;
		}

		public static float ToSimUnits(double displayUnits)
		{
			return (float)displayUnits * _simUnitsToDisplayUnitsRatio;
		}

		public static float ToSimUnits(int displayUnits)
		{
			return displayUnits * _simUnitsToDisplayUnitsRatio;
		}

		public static Vector2 ToSimUnits(Vector2 displayUnits)
		{
			return displayUnits * _simUnitsToDisplayUnitsRatio;
		}

		public static Vector3 ToSimUnits(Vector3 displayUnits)
		{
			return displayUnits * _simUnitsToDisplayUnitsRatio;
		}

		public static void ToSimUnits(ref Vector2 displayUnits, out Vector2 simUnits)
		{
			Vector2.Multiply(ref displayUnits, _simUnitsToDisplayUnitsRatio, out simUnits);
		}

		public static Vector2 ToSimUnits(float x, float y)
		{
			return new Vector2(x, y) * _simUnitsToDisplayUnitsRatio;
		}

		public static Vector2 ToSimUnits(double x, double y)
		{
			return new Vector2((float)x, (float)y) * _simUnitsToDisplayUnitsRatio;
		}

		public static void ToSimUnits(float x, float y, out Vector2 simUnits)
		{
			simUnits = Vector2.Zero;
			simUnits.X = x * _simUnitsToDisplayUnitsRatio;
			simUnits.Y = y * _simUnitsToDisplayUnitsRatio;
		}
	}
}
