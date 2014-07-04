using AIRLab.Mathematics;
using AIRLab.Thornado;
using CVARC.Core;

namespace CVARC.Graphics
{
	[Thornado]
	public class DrawerSettings
	{
		[Thornado]
		public Frame3D TopViewCameraLocation = new Frame3D(150, -100, 300);

		[Thornado]
		public bool FullScreen;

		[Thornado]
		public ViewModes ViewMode = ViewModes.Trackball;

		[Thornado]
		public bool ShowControls = true;

		[Thornado]
		public Body Robot;

		[Thornado]
		public Frame3D BodyCameraLocation = new Frame3D(0, 0, 20, Angle.FromGrad(-35), Angle.Zero, Angle.Zero);
	}
}