using AIRLab.Mathematics;

using CVARC.Core;

namespace CVARC.Graphics
{

	public class DrawerSettings
	{

		public Frame3D TopViewCameraLocation = new Frame3D(150, -100, 300);


		public bool FullScreen;


		public ViewModes ViewMode = ViewModes.Trackball;


		public bool ShowControls = true;


		public Body Robot;


		public Frame3D BodyCameraLocation = new Frame3D(0, 0, 20, Angle.FromGrad(-35), Angle.Zero, Angle.Zero);
	}
}