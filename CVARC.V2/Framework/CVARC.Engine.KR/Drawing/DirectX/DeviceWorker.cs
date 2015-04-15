using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using CVARC.Graphics.DirectX.Utils;
using SlimDX;
using SlimDX.Direct3D9;

namespace CVARC.Graphics.DirectX
{
	internal partial class DeviceWorker
	{
		internal DeviceWorker()
			: this(new DeviceSettings())
		{
		}

		internal DeviceWorker(DeviceSettings deviceSettings)
		{
			_deviceSettings = deviceSettings;
			lock(DeviceLock)
				_thread.Enqueue(Initialize);
		}

		public event Action BeforeReset;
		public event Action AfterReset;
		public event Action Disposing;

		public bool ResetIfRequired(IDirectXDrawer drawer, Size updatingSize)
		{
			CheckSize(updatingSize);
			lock(DeviceLock)
			{
				_requestedSizes[drawer] = updatingSize;
				int maxWidth = _requestedSizes.Max(x => x.Value.Width);
				int maxHeight = _requestedSizes.Max(x => x.Value.Height);
				if(maxWidth != _deviceSettings.PresentParameters.BackBufferWidth ||
					maxHeight != _deviceSettings.PresentParameters.BackBufferHeight)
				{
					ResetInternal(maxWidth, maxHeight);
					return true;
				}
				return false;
			}
		}

		public void ClearDevice()
		{
			ClearFlags clearFlags = _deviceSettings.PresentParameters.EnableAutoDepthStencil ? ClearFlags.All : ClearFlags.Target;
			Device.Clear(clearFlags, Color.White, 1.0f, 0);
		}

		public SwapChain GetSwapChainUnsafe(IDirectXDrawer drawer, Control control)
		{
			Size requestedSize = control.ClientSize;
			CheckSize(requestedSize);
			lock(DeviceLock)
			{
				_requestedSizes[drawer] = requestedSize;
				int width = requestedSize.Width;
				int height = requestedSize.Height;
				PresentParameters paramsCopy = _deviceSettings.PresentParameters.Clone();
				var hndl = GetControl(control);
			    paramsCopy.DeviceWindowHandle = hndl;
				paramsCopy.BackBufferWidth = width;
				paramsCopy.BackBufferHeight = height;
				return new SwapChain(Device, paramsCopy);
			}
		}
        private Dictionary<Control, IntPtr> hash = new Dictionary<Control, IntPtr>();
	    private IntPtr GetControl(Control control)
	    {
	        if(!hash.ContainsKey(control))
	            hash.Add(control, control.Handle);
            return hash[control];
	    }

	    public Surface GetRenderSurfaceUnsafe(IDirectXDrawer drawer, Size size)
		{
			CheckSize(size);
			lock(DeviceLock)
			{
				_requestedSizes[drawer] = size;
				Format format = _deviceSettings.PresentParameters.BackBufferFormat;
				MultisampleType multisampleType = _deviceSettings.PresentParameters.Multisample;
				int multisampleQuality = _deviceSettings.PresentParameters.MultisampleQuality;
				return Surface.CreateRenderTarget(Device, size.Width, size.Height,
												format, multisampleType, multisampleQuality, false);
			}
		}

		public void ReleaseDrawer(IDirectXDrawer drawer)
		{
			lock(DeviceLock)
				_requestedSizes.Remove(drawer);
		}

		public bool HandleIfDeviceLost()
		{
			Result result = Device.TestCooperativeLevel();
			if(result == ResultCode.DeviceLost)
			{
				Thread.Sleep(50);
				return true;
			}
			if(result == ResultCode.DeviceNotReset)
			{
				ResetInternal(_deviceSettings.PresentParameters.BackBufferWidth, _deviceSettings.PresentParameters.BackBufferHeight);
				return true;
			}
			return false;
		}

		public void TryDispose()
		{
			//Console.WriteLine("Drawers left: {0}", _requestedSizes.Count);
			if(_requestedSizes.Count == 0)
				DisposeInternal();
		}

		public Device Device { get; private set; }
		public readonly object DeviceLock = new object();

		internal Size DeviceSize { get { return new Size(_deviceSettings.PresentParameters.BackBufferWidth, _deviceSettings.PresentParameters.BackBufferHeight); } }

		internal class DeviceSettings
		{
			public bool AutoDetermineMultisampleType = true;
			public MultisampleType MultisampleType = MultisampleType.None;
			public PresentParameters PresentParameters;
		}

		private static void CheckSize(Size size)
		{
			if(size.Width <= 0 || size.Height <= 0)
				throw new ArgumentException("Invalid size");
		}

		private void DisposeInternal()
		{
			if(Disposing != null)
				Disposing();
			lock(DeviceLock)
				_thread.Enqueue(() => Device.Dispose());
			LogInfo("Device worker disposed");
		}

		private void ResetInternal(int width, int height)
		{
			LogInfo("Beginning device reset");
			if(BeforeReset != null)
				BeforeReset();
			_thread.Enqueue(() =>
				{
					_deviceSettings.PresentParameters.BackBufferHeight = height;
					_deviceSettings.PresentParameters.BackBufferWidth = width;
					Device.Reset(_deviceSettings.PresentParameters);
				});
			if(AfterReset != null)
				AfterReset();
			LogInfo("Resized device to {0} x {1}", width, height);
		}

		private static void LogInfo(string message, params object[] args)
		{
			//Console.WriteLine(message, args);
		}

		private void Initialize()
		{
			_stopwatch.Start();
			var form = new Form();
			_deviceSettings.PresentParameters = new PresentParameters
				{
					BackBufferHeight = 10,
					BackBufferWidth = 10,
					DeviceWindowHandle = form.Handle,
					EnableAutoDepthStencil = false
				};
			using(var d3D = new Direct3D())
				CreateDevice(d3D, _deviceSettings);
			SwapChain swapChain = null;
			Surface buffer = null;
			try
			{
				swapChain = Device.GetSwapChain(0);
				buffer = swapChain.GetBackBuffer(0);
			}
			finally
			{
				Device.GetRenderTarget(0).Dispose();
				if(swapChain != null) swapChain.Dispose();
				if(buffer != null) buffer.Dispose();
				form.Dispose();
			}
			_stopwatch.Stop();
			LogInfo("Device initialized in {0} ms", _stopwatch.ElapsedMilliseconds);
		}

		private readonly DeviceSettings _deviceSettings;
		private readonly Stopwatch _stopwatch = new Stopwatch();
		private readonly ThreadActionQueue _thread = new ThreadActionQueue();
		private readonly Dictionary<IDirectXDrawer, Size> _requestedSizes = new Dictionary<IDirectXDrawer, Size>();

		#region Initializing a software renderer

		private static bool TryRegisterSoftwareRenderer(Direct3D d3D)
		{
			var library = new IntPtr();
			foreach(string suffix in new[] {"", "_1", "_2"})
			{
				string lpFileName = Path.Combine(Environment.SystemDirectory, "rgb9rast" + suffix + ".dll");
				library = LoadLibrary(lpFileName);
				if(library.ToInt32() != 0)
					break;
			}
			if(library.ToInt32() == 0)
				return false;
			var procedure = unchecked((IntPtr)(long)(ulong)GetProcAddress(library, "D3D9GetSWInfo"));
			d3D.RegisterSoftwareDevice(procedure);
			return true;
		}

		[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
		private static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);

		#endregion

		#region Creating a device

		private void CreateDevice(Direct3D d3D, DeviceSettings deviceSettings)
		{
			Device device;
			if(!TryCreateHardwareDevice(d3D, deviceSettings, out device) &&
				!TryCreateSoftwareDevice(d3D, deviceSettings, out device))
				CreateReferenceDevice(d3D, deviceSettings, out device);
			Device = device;
		}

		private static bool TryCreateHardwareDevice(Direct3D d3D, DeviceSettings deviceSettings, out Device device)
		{
			return TryCreateDevice(d3D, deviceSettings, DeviceType.Hardware, CreateFlags.HardwareVertexProcessing, out device);
		}

		private static bool TryCreateSoftwareDevice(Direct3D d3D, DeviceSettings deviceSettings, out Device device)
		{
			device = null;
			return TryRegisterSoftwareRenderer(d3D) &&
					TryCreateDevice(d3D, deviceSettings, DeviceType.Software, CreateFlags.SoftwareVertexProcessing, out device);
		}

		private static void CreateReferenceDevice(Direct3D d3D, DeviceSettings deviceSettings, out Device device)
		{
			if(!TryCreateDevice(d3D, deviceSettings, DeviceType.Reference, CreateFlags.SoftwareVertexProcessing, out device))
				throw new Exception("Unable to create any direct3d device");
		}

		private static bool TryCreateDevice(Direct3D d3D, DeviceSettings deviceSettings, DeviceType deviceType,
											CreateFlags vertexProcessingFlag, out Device device)
		{
			Format adapterformat = d3D.Adapters[0].CurrentDisplayMode.Format;
			//для теней обязательно нужен stencil buffer. Ищем формат
			PresentParameters presentParameters = deviceSettings.PresentParameters;
			foreach(Format stencilformat in new[] {Format.D24S8, Format.D24SingleS8, Format.D24X4S4, Format.D15S1})
			{
				if(d3D.CheckDeviceFormat(0, deviceType, adapterformat,
										Usage.DepthStencil, ResourceType.Surface, stencilformat) &&
					d3D.CheckDepthStencilMatch(0, deviceType,
												adapterformat, adapterformat, stencilformat))
				{
					presentParameters.EnableAutoDepthStencil = true;
					presentParameters.AutoDepthStencilFormat = stencilformat;
					break;
				}
			}
			//Выбираем тип multisampling
			var multiSamplingTypesToCheck = new Dictionary<MultisampleType, int>();
			if(deviceSettings.AutoDetermineMultisampleType)
				multiSamplingTypesToCheck = new[] {MultisampleType.FourSamples, MultisampleType.TwoSamples, MultisampleType.None}.ToDictionary(x => x, x => (int)x);
			else
			{
				MultisampleType multType = deviceSettings.MultisampleType;
				multiSamplingTypesToCheck.Add(multType, (int)multType);
			}
			foreach(var i in multiSamplingTypesToCheck)
			{
				presentParameters.Multisample = i.Key;
				presentParameters.MultisampleQuality = i.Value;
				try
				{
					device = new Device(d3D, 0, deviceType, presentParameters.DeviceWindowHandle,
										CreateFlags.Multithreaded | vertexProcessingFlag, presentParameters);
					return true;
				}
				catch(Direct3D9Exception)
				{
				}
			}
			device = null;
			return false;
		}

		#endregion
	}
}