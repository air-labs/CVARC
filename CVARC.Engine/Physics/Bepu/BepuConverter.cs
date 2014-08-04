using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics.MathExtensions;
using AIRLab.Mathematics;
using BEPUphysics.Entities;

namespace CVARC.Core.Physics.BepuWrap
{
	/// <summary>
	/// Convert units between display and simulation units.
	/// </summary>
	public static class BepuConverter
	{
		private static float _displayUnitsToSimUnitsRatio = 10f;
		private static float _simUnitsToDisplayUnitsRatio = 1 / _displayUnitsToSimUnitsRatio;

		//-------------------------------------------------------------------------

		#region Frame3D convertions

		/// <summary>
		/// Вернёт Frame3D, не конвертируя его в координаты экрана.
		/// </summary>
		public static Frame3D Vector3ToFrame3D(Vector3 v)
		{
			return new Frame3D(v.X, v.Y, v.Z);
		}

		/// <summary>
		/// Вернёт Frame3D, не конвертируя его в координаты физики.
		/// </summary>
		public static Vector3 Frame3DToVector3(Frame3D f)
		{
			return new Vector3((float)f.X, (float)f.Y, (float)f.Z);
		}

		/// <summary>
		/// Запишет координаты объекта в frame, переведя их в координаты экрана.
		/// </summary>
		public static Frame3D GetFrame(Entity entity, Quaternion inititalOrientation)
		{
			inititalOrientation = new Quaternion(inititalOrientation.X, inititalOrientation.Y, inititalOrientation.Z, -inititalOrientation.W);

			Frame3D f = Vector3ToFrame3D(ToDisplayUnits(entity.Position));
			f = f.NewX(-f.X);

			BEPUphysics.MathExtensions.Matrix m;
			Quaternion q = entity.Orientation;
			BEPUphysics.MathExtensions.Matrix.CreateFromQuaternion(ref q, out m);

			f = SetAngles(f, m);
			Quaternion orient = entity.Orientation * inititalOrientation;
			f.Orientation = new Quat(orient.X, orient.Y, orient.Z, orient.W);
			return f;
		}

		/// <summary>
		/// Поставит углы, схватит gimbal lock у pitch, но пока так, хоть Yaw верный.
		/// </summary>
		private static Frame3D SetAngles(Frame3D frame, BEPUphysics.MathExtensions.Matrix matrix)
		{
			AIRLab.Mathematics.Matrix m = new AIRLab.Mathematics.Matrix(4, 4); 
			m[0, 0] = matrix.M11;
			m[0, 1] = matrix.M12;
			m[0, 2] = matrix.M13;
			m[0, 3] = 0.0f;
			m[1, 0] = matrix.M21;
			m[1, 1] = matrix.M22;
			m[1, 2] = matrix.M23;
			m[1, 3] = 0.0f;
			m[2, 0] = matrix.M31;
			m[2, 1] = matrix.M32;
			m[2, 2] = matrix.M33;
			m[2, 3] = 0.0f;
			m[3, 0] = frame.X;
			m[3, 1] = frame.Y;
			m[3, 2] = frame.Z;
			m[3, 3] = 1.0f;

			var yaw = Geometry.Atan2(m[1, 0], m[0, 0]);
            var pitch = Geometry.Atan2(-m[2, 0], Geometry.Hypot(m[2, 1], m[2, 2]));
            var roll = Geometry.Atan2(m[2, 1], m[2, 2]);

			Frame3D fr = new Frame3D(frame.X, frame.Y, frame.Z, pitch, yaw, roll); 

			return fr;
		}

		/// <summary>
		/// Засунет объект в фрейм, переведя в координаты физики, повернув объект на нужные углы.
		/// </summary>
		public static void PutInFrame(Entity entity, Frame3D frame, Quaternion inititalOrientation)
		{			
			entity.Position = ToSimUnits(Frame3DToVector3(frame.NewX(-frame.X)));
			
			entity.Orientation = Quaternion.CreateFromYawPitchRoll(
						//(float)frame.Pitch.Radian, (float)frame.Roll.Radian, -(float)frame.Yaw.Radian);
						(float)frame.Roll.Radian, (float)frame.Pitch.Radian, -(float)frame.Yaw.Radian) * inititalOrientation;
		}

		#endregion

		//-------------------------------------------------------------------------

		#region Sim and display convertions

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

		#endregion
	}
}
