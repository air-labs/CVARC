using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using SlimDX;
using SlimDX.Direct3D9;

namespace CVARC.Graphics.DirectX
{
	public class OffscreenDirectXDrawer : IDirectXDrawer
	{
		public OffscreenDirectXDrawer(DirectXScene scene,
									int width, int height, ImageFileFormat imageFileFormat)
		{
			_stopwatch.Restart();
			_scene = scene;
			_effect = scene.Effect;
			_deviceWorker = scene.DeviceWorker;
			_device = _deviceWorker.Device;
			_size = new Size(width, height);
			AcquireSurfaces();
			_deviceWorker.BeforeReset += DisposeSurfaces;
			_deviceWorker.AfterReset += AcquireSurfacesUnsafe;
			_deviceWorker.Disposing += DisposeSurfaces;
			_imageFileFormat = imageFileFormat;
			_stopwatch.Stop();
			LogInfo("Created offscreen drawer in {0}", _stopwatch.ElapsedMilliseconds);
		}

		public OffscreenDirectXDrawer(DirectXScene scene, int width, int height)
			: this(scene, width, height, ImageFileFormat.Png)
		{
		}

		public bool TryGetImage(Camera camera, out byte[] bitmapResult)
		{
			bitmapResult = null;
			_stopwatch.Restart();
			lock(_deviceWorker.DeviceLock)
			{
				if(_disposed || _device.Disposed)
					return false;
				RenderScene(camera);
				if(_renderSurface.Description.MultisampleType != MultisampleType.None)
				{
					_device.StretchRectangle(_renderSurface, _surfaceNonAliased, TextureFilter.Linear);
					_device.GetRenderTargetData(_surfaceNonAliased, _offSurface);
				}
				else
					_device.GetRenderTargetData(_renderSurface, _offSurface);
				DataStream stream = Surface.ToStream(_offSurface, _imageFileFormat);
				byte[] bytes = ToByteArray(stream);
				_stopwatch.Stop();
				LogInfo("Offscreen image rendered in {0} ms", _stopwatch.ElapsedMilliseconds);
				bitmapResult = bytes;
				return true;
			}
		}

		public void Dispose()
		{
			lock(_deviceWorker.DeviceLock)
			{
				if (_disposed)
					return;
				LogInfo("Disposing offscreen drawer");
				_deviceWorker.BeforeReset -= DisposeSurfaces;
				_deviceWorker.AfterReset -= AcquireSurfacesUnsafe;
				_deviceWorker.Disposing -= DisposeSurfaces;
				DisposeSurfaces();
				_deviceWorker.ReleaseDrawer(this);
				_deviceWorker.TryDispose();
				_disposed = true;
			}
		}

		private void AcquireSurfaces()
		{
			_deviceWorker.ResetIfRequired(this, _size);
			AcquireSurfacesUnsafe();
		}

		private void DisposeSurfaces()
		{
			_renderSurface.Dispose();
			_surfaceNonAliased.Dispose();
			_offSurface.Dispose();
		}

		private void AcquireSurfacesUnsafe()
		{
			_renderSurface = _deviceWorker.GetRenderSurfaceUnsafe(this, _size);
			_surfaceNonAliased = Surface.CreateRenderTarget(_device, _size.Width, _size.Height,
															_renderSurface.Description.Format, MultisampleType.None, 0, true);
			_offSurface = Surface.CreateOffscreenPlain(_device, _size.Width, _size.Height,
														_renderSurface.Description.Format, Pool.SystemMemory);
		}

		private void RenderScene(Camera camera)
		{
			_deviceWorker.Device.SetRenderTarget(0, _renderSurface);
			_deviceWorker.ClearDevice();
			_effect.ProjectionTransform = camera.ProjectionTransform;
			_effect.ViewTransform = camera.ViewTransform;
			_effect.WorldTransform = camera.WorldTransform;
			_effect.DrawScene(_scene);
		}

		private static byte[] ToByteArray(Stream input)
		{
			var buffer = new byte[input.Length];
			input.Read(buffer, 0, (int)input.Length);
			input.Dispose();
			return buffer;
		}

		private static void LogInfo(string message, params object[] args)
		{
			//Console.WriteLine(message, args);
		}

		private bool _disposed;
		private readonly Stopwatch _stopwatch = new Stopwatch();
		private readonly DirectXScene _scene;

		private readonly Device _device;
		private Surface _offSurface;
		private Surface _surfaceNonAliased;
		private Surface _renderSurface;
		private readonly ImageFileFormat _imageFileFormat;

		private readonly DeviceWorker _deviceWorker;
		private readonly Effect _effect;
		private readonly Size _size;
	}
}