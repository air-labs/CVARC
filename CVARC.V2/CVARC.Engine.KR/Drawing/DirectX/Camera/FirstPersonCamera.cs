using AIRLab.Mathematics;
using CVARC.Core;
using SlimDX;
using Matrix = SlimDX.Matrix;

namespace CVARC.Graphics.DirectX
{
	/// <summary>
	/// Камера с видом от первого лица
	/// </summary>
	public class FirstPersonCamera : Camera
	{
		/// <summary>
		/// Создает камеру с видом от первого лица
		/// </summary>
		/// <param name="source">Тело, к которому привязана камера</param>
		/// <param name="offset">Смещение камеры относительно Location тела</param>
		/// <param name="viewAngle"> </param>
		/// <param name="aspectRatio"> </param>
		public FirstPersonCamera(Body source, Frame3D offset,
			Angle viewAngle, double aspectRatio) : base(aspectRatio)
		{
			_source = source;
			_offset = offset;
			ViewAngle = viewAngle;
			_defaultWorldMatrix = Matrix.LookAtLH(Vector3.Zero, new Vector3(-1, 0, 0), UpVector);
		}

		public override Matrix ViewTransform
		{
			get
			{
				Matrix m1 = _offset.ToDirectXMatrix();
				m1.Invert();
				Matrix m2 = _source.GetAbsoluteLocationMatrix();
				m2.Invert();
				return m2 * (m1 * _defaultWorldMatrix);
			}
		}

		private readonly Body _source;
		private readonly Frame3D _offset;
		private readonly Matrix _defaultWorldMatrix;
	}
}