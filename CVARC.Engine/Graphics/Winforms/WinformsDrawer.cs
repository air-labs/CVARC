using System;
using System.Threading;
using System.Windows.Forms;
using AIRLab.Mathematics;
using CVARC.Core;

namespace CVARC.Graphics.Winforms
{
	public class WinformsDrawer : FormDrawer
	{
		/// <summary>
		/// Создает Drawer с использованием заданных настроек и корневого тела
		/// </summary>
		/// <param name="root">Корень дерева тел</param>
		/// <param name="settings">Настройки</param>
		public WinformsDrawer(Body root, DrawerSettings settings)
		{
			_settings = settings;
			_root = root;
		}

		/// <summary>
		/// Создает Drawer c использованием переданного корневого тела,
		/// дефолтными настройками, отрисовывающий в пустую форму
		/// </summary>
		/// <param name="root">Корень дерева тел</param>
		public WinformsDrawer(Body root)
			: this(root, new DrawerSettings())
		{
		}

		public override void StartDrawing(Control control)
		{
			_control = control;
			_pictureBox = new PictureBox
				{
					Size = control.ClientSize
				};
			_pictureBox.Paint += FormPaint;
			control.Controls.Add(_pictureBox);
			_pictureBox.SendToBack();
			_scene = new WinformsScene();
			_timer = new System.Threading.Timer(Tick, null, 0, 1000 / FrameRate);
			_initialized.Set();
		}

		public override void ResizeToFit()
		{
			if(_pictureBox != null)
				_pictureBox.Size = _control.ClientSize;
		}

		public override void Dispose()
		{
			_timer.Dispose();
			if(_graphics != null)
				_graphics.Dispose();
			if(_graphics != null)
				_pictureBox.Dispose();
		}

		public override void WaitForInitialization()
		{
			_initialized.Wait();
		}

		private float GetScalingFactor()
		{
			var worldVisible = (float)(_settings.TopViewCameraLocation.Z *
                                        Geometry.Tg(SceneConfig.ThirdPersonViewAngle / 2) * 2);
			return Math.Min(_control.ClientSize.Width / worldVisible,
							_control.ClientSize.Height / worldVisible);
		}

		private void FormPaint(object sender, PaintEventArgs e)
		{
			_graphics = e.Graphics;
			_scene.Graphics = _graphics;
			_graphics.ResetTransform();
			_graphics.TranslateTransform((float)_control.ClientSize.Width / 2,
										(float)_control.ClientSize.Height / 2);
			float scalingFactor = GetScalingFactor();
			_graphics.ScaleTransform(scalingFactor, scalingFactor);
			_scene.UpdateModels(_root);
		}

		private void Tick(object state)
		{
			_pictureBox.Invalidate();
		}

		private readonly DrawerSettings _settings;

		private WinformsScene _scene;
		private System.Drawing.Graphics _graphics;
		private readonly Body _root;
		private System.Threading.Timer _timer;
		private PictureBox _pictureBox;
		private Control _control;
		private readonly ManualResetEventSlim _initialized = new ManualResetEventSlim();
		private const int FrameRate = 20;
	}
}