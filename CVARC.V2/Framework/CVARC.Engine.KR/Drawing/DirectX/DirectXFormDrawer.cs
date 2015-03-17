using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SlimDX.Direct3D9;

namespace CVARC.Graphics.DirectX
{
	/// <summary>
	/// Drawer в DirectX - штука, рисующая сцену с определенного ракурса,
	/// с некоторыми настройками.
	/// </summary>
	public partial class DirectXFormDrawer : FormDrawer, IDirectXDrawer, IDisposable
	{
		/// <summary>
		/// Создает новый Drawer c использованием переданных сцены, настроек
		/// и функции-инициализатора для окна, в которое следует отрисовывать
		/// </summary>
		/// <param name="scene">Сцена, которую следует отрисовывать</param>
		/// <param name="settings">Настройки</param>
		public DirectXFormDrawer(DirectXScene scene, DrawerSettings settings)
		{
			_settings = settings;
			_scene = scene;
			_effect = scene.Effect;
			_deviceWorker = scene.DeviceWorker;
			//TODO! Return camera controls.
			/*if(Settings.ShowControls)
				Control.Controls.Add(new CameraSwitchControl(_camera));*/
		}

		public DirectXFormDrawer(DirectXScene directXScene)
			: this(directXScene, new DrawerSettings())

		{
		}

		public override void Dispose()
		{
			lock(_deviceWorker.DeviceLock)
			{
				if(Disposed)
					return;
				LogInfo("Disposing form drawer");
				_timer.Stop();
				_deviceWorker.BeforeReset -= DisposeSurfaces;
				_deviceWorker.AfterReset -= AcquireSurfacesUnsafe;
				DisposeSurfaces();
				_deviceWorker.ReleaseDrawer(this);
				_deviceWorker.TryDispose();
				Disposed = true;
				_started = false;
			}
		}

	

		public override void StartDrawing(Control control)
		{
			try
			{
				if(_started)
					return;
				_started = true;
				_control = control;
				_deviceWorker.ResetIfRequired(this, control.ClientSize);
				AcquireSurfacesUnsafe(control);
				_deviceWorker.BeforeReset += DisposeSurfaces;
				_deviceWorker.AfterReset += () => AcquireSurfacesUnsafe(control);
				_camera = new SwitchableCamera(_settings.Robot, _settings.BodyCameraLocation,
												control, _settings.TopViewCameraLocation)
					{
						AspectRatio = GetAspectRatio(control),
						Mode = _settings.ViewMode
					};
				StartShowingFrames();
				_initialized.Set();
			}
			catch(Exception)
			{
				Dispose();
				throw;
			}
		}

		public override void ResizeToFit()
		{
			try
			{
				if(!_started)
					return;
				_camera.AspectRatio = GetAspectRatio(_control);
				LogInfo("Resizing drawer to control size ({0})", _control.ClientSize);
				var topLevelForm = _control.TopLevelControl as Form;
				if(topLevelForm.WindowState == FormWindowState.Minimized)
					return;
				if(_deviceWorker.ResetIfRequired(this, _control.ClientSize))
					return;
				LogInfo("Performing local reset");
				DisposeSurfaces();
				AcquireSurfacesUnsafe(_control);
			}
			catch(Exception)
			{
				Dispose();
				throw;
			}
		}

		public bool Disposed { get; private set; }

		public Size Size { get { return new Size(_buffer.Description.Width, _buffer.Description.Height); } }
		public double AspectRatio { get { return Size.Width / (double)Size.Height; } }

		public override void WaitForInitialization()
		{
			_initialized.Wait();
		}

		private static double GetAspectRatio(Control control)
		{
			return control.ClientSize.Width / (double)control.ClientSize.Height;
		}

		private static int GetFramerate(DeviceType deviceType)
		{
			return deviceType == DeviceType.Hardware ? SceneConfig.Framerate : 15;
		}

		private void AcquireSurfacesUnsafe()
		{
			AcquireSurfacesUnsafe(_control);
		}

		private void StartShowingFrames()
		{
			_framerate = GetFramerate(_scene.DeviceWorker.Device.CreationParameters.DeviceType);
			_timer = new System.Windows.Forms.Timer
				{
					Interval = 1000 / _framerate
				};
			_timer.Tick += (o, args) => ShowOneFrame();
			_timer.Start();
		}

		private void ShowOneFrame()
		{
			try
			{
				lock(_deviceWorker.DeviceLock)
				{
					RenderScene(_camera);
					if(!_deviceWorker.HandleIfDeviceLost())
						_swapChain.Present(Present.None);
				}
			}
			catch(Direct3D9Exception)
			{
				Dispose();
			}
		}

		private void RenderScene(Camera camera)
		{
			_deviceWorker.Device.SetRenderTarget(0, _buffer);
			_deviceWorker.ClearDevice();
			_effect.ProjectionTransform = camera.ProjectionTransform;
			_effect.ViewTransform = camera.ViewTransform;
			_effect.WorldTransform = camera.WorldTransform;
			_effect.DrawScene(_scene);
		}

		private void AcquireSurfacesUnsafe(Control control)
		{
			LogInfo("Acquiring surfaces");
			_swapChain = _deviceWorker.GetSwapChainUnsafe(this, control);
			_buffer = _swapChain.GetBackBuffer(0);
		}

		private void DisposeSurfaces()
		{
			LogInfo("Disposing surfaces");
			if(_buffer != null && !_buffer.Disposed)
				_buffer.Dispose();
			if(_swapChain != null && !_swapChain.Disposed)
				_swapChain.Dispose();
		}

		private static void LogInfo(string message, params object[] args)
		{
			//Console.WriteLine(message, args);
		}

		private SwitchableCamera _camera;

		private readonly DeviceWorker _deviceWorker;
		private System.Windows.Forms.Timer _timer;
		private bool _started;
		private readonly Effect _effect;
		private readonly DirectXScene _scene;
		private SwapChain _swapChain;
		private Surface _buffer;
		private int _framerate;
		private Control _control;
		private readonly ManualResetEventSlim _initialized = new ManualResetEventSlim();
		private readonly DrawerSettings _settings;
	}
}