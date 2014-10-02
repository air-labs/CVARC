using System.Drawing;
using System.Windows.Forms;
using AIRLab.Mathematics;
using CVARC.Core;
using NUnit.Framework;

namespace CVARC.Graphics.DirectX
{
	class ControlPlacementTests
	{
		private Form _form;
		
		[TearDown]
		public void TearDown()
		{
			_form.EasyInvoke(x=>x.Close());
		}

		[Test]
		public void TwoControlsPlacement()
		{
			_form = new Form {ClientSize = new Size(800, 600)};
			var cameraSwitch = new CameraSwitchControl(new SwitchableCamera(new Body(), Frame3D.Identity, _form, Frame3D.Identity));

			_form.Controls.Add(cameraSwitch);
			var scoreDisplay = new ScoreDisplayControl();

			_form.Controls.Add(scoreDisplay);
			CheckAnchoredRight(scoreDisplay);
			CheckAnchoredLeft(cameraSwitch);

		}

		private void CheckAnchoredRight(Control control)
		{
			Assert.AreEqual(_form.ClientSize.Width, control.Right);
			Assert.AreEqual(_form.ClientSize.Width - control.ClientSize.Width, control.Left);
			Assert.AreEqual(control.ClientSize.Height/2, control.Top);
		}

		private void CheckAnchoredLeft(Control control)
		{
			Assert.AreEqual(0, control.Left);
			Assert.That(control.Right>0);
			Assert.AreEqual(0, control.Top);
		}
	}
}
