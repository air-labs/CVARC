using AIRLab.Mathematics;
using SlimDX;
using Matrix = SlimDX.Matrix;

namespace CVARC.Graphics.DirectX
{
	/// <summary>
	/// Камера с видом свержу
	/// </summary>
	public class TopViewCamera : Camera
	{
		public TopViewCamera(Frame3D loc, double aspectRatio) : base(aspectRatio)
		{
			Vector3 vec = loc.ToDirectXVector();
			_topViewTransform = Matrix.LookAtLH(new Vector3(0, 0, vec.Z),
			                                    new Vector3(), Vector3.UnitY);
		}

		public override Matrix ViewTransform { get { return _topViewTransform; } }
		private readonly Matrix _topViewTransform;
	}
}