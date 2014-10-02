using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AIRLab.Mathematics;
using CVARC.Core;
using Matrix = SlimDX.Matrix;

namespace CVARC.Graphics.DirectX
{
	/// <summary>
	/// Переключаемая камера.
	/// </summary>
	public class SwitchableCamera : Camera
	{
		public SwitchableCamera(
			Body sourceBody, Frame3D bodyOffset, Control form, Frame3D location)
			: base(form.ClientSize.Width/(double) form.ClientSize.Height)
		{
			double aspectRatio = form.ClientSize.Width/(double) form.ClientSize.Height;
			AddMode(ViewModes.Top, new TopViewCamera(location, aspectRatio));
			AddMode(ViewModes.Trackball, new TrackballCamera(form, location));
			if (sourceBody != null)
				AddMode(ViewModes.FirstPerson,
				        new FirstPersonCamera(sourceBody, bodyOffset,
				                              SceneConfig.FirstPersonViewAngle, aspectRatio));
		}

		public ViewModes Mode
		{
			get { return _mode; }
			set { _mode = _cameras.ContainsKey(value) ? value : _cameras.First().Key; }
		}

		public override double AspectRatio
		{
			get { return _cameras[ViewModes.Top].AspectRatio; }
			set
			{
				foreach (var camera in _cameras)
					camera.Value.AspectRatio = value;
			}
		}

		public override Matrix ViewTransform
		{
			get { return _cameras[_mode].ViewTransform; }
		}

		public override Matrix ProjectionTransform
		{
			get { return _cameras[_mode].ProjectionTransform; }
		}

		//public override SlimDX.Matrix WorldTransform { get { return _cameras[_mode].WorldTransform; } }

		private void AddMode(ViewModes mode, Camera camera)
		{
			_cameras.Add(mode, camera);
		}

		private readonly SortedDictionary<ViewModes, Camera> _cameras = new SortedDictionary<ViewModes, Camera>();
		private ViewModes _mode = ViewModes.Top;
	}
}