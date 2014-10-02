using System;
using System.Windows.Forms;
using CVARC.Graphics.DirectX;

namespace CVARC.Graphics
{
	public partial class CameraSwitchControl : UserControl
	{
		public CameraSwitchControl(SwitchableCamera camera)
			: this()
		{
			_camera = camera;
		}

		private CameraSwitchControl()
		{
			InitializeComponent();
		}

		private void ButtonCheckedChanged(object sender, EventArgs e)
		{
			if(_camera == null)
				return;
			RadioButton button = null;
			if(sender is RadioButton)
				button = sender as RadioButton;
			if(button == null || !button.Checked)
				return;
			if(button.Equals(TopViewRadioButton))
				_camera.Mode = ViewModes.Top;
			else if(button.Equals(FollowRobotRadioButton))
				_camera.Mode = ViewModes.FirstPerson;
			else
				_camera.Mode = ViewModes.Trackball;
		}

		private readonly SwitchableCamera _camera;
	}
}