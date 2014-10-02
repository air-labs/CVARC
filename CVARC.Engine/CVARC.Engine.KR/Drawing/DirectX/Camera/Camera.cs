using AIRLab.Mathematics;
using SlimDX;
using Matrix = SlimDX.Matrix;

namespace CVARC.Graphics.DirectX
{
	public abstract class Camera : ICamera<Matrix>
	{
		protected Camera(double aspectRatio)
		{
			Init(aspectRatio);
		}

		public virtual double AspectRatio
		{
			get { return _aspectRatio; }
			set { Init(value); }
		}

		public virtual Matrix ViewTransform
		{
			get { return Matrix.Identity; }
		}

		public virtual Matrix WorldTransform
		{
			get { return Matrix.Identity; }
		}

		public virtual Matrix ProjectionTransform
		{
			get { return _projectionMatrix; }
		}

		protected static readonly Vector3 UpVector = Vector3.UnitZ;

		/// <summary>
		/// Угол обзора
		/// </summary>
		protected Angle ViewAngle = SceneConfig.ThirdPersonViewAngle;

		private void Init(double aspectRatio)
		{
			_aspectRatio = aspectRatio;
			_projectionMatrix = Matrix.PerspectiveFovLH(
				(float) ViewAngle.Radian,
				(float) _aspectRatio, SceneConfig.NearClipDistance,
				SceneConfig.FarClipDistance);
		}

		private Matrix _projectionMatrix = Matrix.Identity;

		private double _aspectRatio;
	}
}