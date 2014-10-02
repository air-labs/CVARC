using System;
using System.Drawing;
using System.Windows.Forms;
using AIRLab.Mathematics;
using SlimDX;
using Matrix = SlimDX.Matrix;

namespace CVARC.Graphics.DirectX
{
	/// <summary>
	/// Камера с управлением движением мышки
	/// </summary>
	public class TrackballCamera : Camera
	{
		/// <summary>
		/// Создает камеру, которую можно крутить движением мышки
		/// </summary>
		/// <param name="eventSource">Control,с которого обрабатываются события </param>
		/// <param name="cameraLocation">Исходное положение камеры</param>
		public TrackballCamera(Control eventSource,Frame3D cameraLocation)
			: base(eventSource.ClientSize.Width/(double) eventSource.ClientSize.Height)
		{
			EventSource = eventSource;
			Vector3 tempLoc = cameraLocation.ToDirectXVector();
			_cameraLocation = new Vector3(Math.Abs(tempLoc.X), 0, tempLoc.Z);
            Angle ang = Geometry.Atan2(tempLoc.Y, tempLoc.X);
			_rotationMatrix = Matrix.RotationZ(-(float) ang.Radian);
			_radius = _cameraLocation.Length();
			Scale = 1;
		}

		public virtual void OnMouseDown(object sender, MouseEventArgs e)
		{
			EventSource.Capture = true;
			_previousPosition2D = e.Location;
			_previousPosition3D = ProjectToTrackball(
				EventSource.ClientRectangle.Width,
				EventSource.ClientRectangle.Height,
				_previousPosition2D);
		}

		public float Scale { get; private set; }

		public override Matrix ViewTransform
		{
			get { return _rotationMatrix*Matrix.LookAtLH(_cameraLocation/Scale, Vector3.Zero, UpVector); }
		}

		/// <summary>
		/// Control, с которого обрабатываются события
		/// </summary>
		public Control EventSource
		{
			get { return _eventSource; }

			set
			{
				if (_eventSource != null)
				{
					_eventSource.MouseDown -= OnMouseDown;
					_eventSource.MouseUp -= OnMouseUp;
					_eventSource.MouseMove -= OnMouseMove;
					_eventSource.MouseWheel -= OnMouseWheelZoom;
				}
				if (value != null)
				{
					_eventSource = value;
					_eventSource.MouseDown += OnMouseDown;
					_eventSource.MouseUp += OnMouseUp;
					_eventSource.MouseMove += OnMouseMove;
					_eventSource.MouseWheel += OnMouseWheelZoom;
				}
			}
		}

		protected virtual void OnMouseUp(object sender, MouseEventArgs e)
		{
			EventSource.Capture = false;
		}

		protected virtual void OnMouseMove(object sender, MouseEventArgs e)
		{
			Point currentPosition = e.Location;
			if (e.Button == MouseButtons.Left)
				Track(currentPosition);
			_previousPosition2D = currentPosition;
		}

		/// <summary>
		/// По перемещению мышки вычисляет перемешение камеры:
		/// матрицу для поворота и перемещение по вертикальной оси
		/// </summary>
		/// <param name="currentPosition">Положение мышки</param>
		protected virtual void Track(Point currentPosition)
		{
			Vector3 currentPosition3D = ProjectToTrackball(
				_eventSource.ClientRectangle.Width,
				_eventSource.ClientRectangle.Height, currentPosition);
			Vector3 diff = currentPosition3D - _previousPosition3D;
			_rotationMatrix *= Matrix.RotationZ(diff.X);
			float tempz = _cameraLocation.Z + diff.Y*100;
			var tempx = (float) Math.Sqrt(_radius*_radius - tempz*tempz);
			if (tempx > 1 && tempz > 0)
			{
				_cameraLocation.X = tempx;
				_cameraLocation.Z = tempz;
			}
			_previousPosition3D = currentPosition3D;
		}

		protected static Vector3 ProjectToTrackball(double width, double height, Point point)
		{
			double x = point.X/(width/2); // Scale so bounds map to [0,0] - [2,2]
			double y = point.Y/(height/2);
			x = x - 1; // Translate 0,0 to the center
			y = 1 - y; // Flip so +Y is up instead of down
			double z2 = 1 - x*x - y*y; // z^2 = 1 - x^2 - y^2
			double z = z2 > 0 ? Math.Sqrt(z2) : 0;
			return new Vector3((float) x, (float) y, (float) z);
		}

		private void OnMouseWheelZoom(object sender, MouseEventArgs e)
		{
			float yDelta = e.Delta;
			double scale = Math.Exp(yDelta/4000); // e^(yDelta/100) is fairly arbitrary.
			Scale *= (float) scale;
		}

		private Control _eventSource;
		private Vector3 _cameraLocation;
		private Matrix _rotationMatrix = Matrix.Identity;
		private readonly float _radius;
		private Vector3 _previousPosition3D;
		private Point _previousPosition2D;
	}
}