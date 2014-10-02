using System;
using System.Diagnostics;
using System.Drawing;
using CVARC.Core;
using CVARC.Graphics.DirectX.Utils;
using NUnit.Framework;
using SlimDX;
using SlimDX.Direct3D9;

namespace CVARC.Graphics.DirectX
{
	internal class BrushConverterTests
	{
		[SetUp]
		public void SetUp()
		{
			_deviceWorker = new DeviceWorker();
		
			_converter = new DirectXBrushConverter(_deviceWorker.Device);
		}
		[TearDown]
		public void TearDown()
		{
			_deviceWorker.TryDispose();
		}

		[Test]
		public void SolidColorBrush()
		{
			var brush = new SolidColorBrush {Color = Color.Red};
			ExtendedMaterial res = brush.Convert(_converter);
			CheckColorsEqual(brush.Color, res.MaterialD3D.Diffuse);
			ExtendedMaterial res2 = new SolidColorBrush {Color = Color.Yellow}.Convert(_converter);
			CheckMaterial(Color.Yellow, res2.MaterialD3D);
		}

		[Test]
		public void PlaneImageBrushTest()
		{
			var brush =PlaneImageBrush.FromResource(()=>Properties.Resources.testtexture);
			Texture texture;
			var res = _converter.TryConvert(brush, out texture);
			try
			{
				CheckMaterial(DirectXBrushConverter.DefaultColor, res.MaterialD3D);
				Assert.NotNull(texture);
			}
			finally
			{
				texture.Dispose();
			}
			_converter.TryConvert(new SolidColorBrush(), out texture);
			Assert.Null(texture);
		}

		[Test]
		public void SomeUnkownBrush()
		{
			var brush = new FailBrush();
			Assert.Throws<Exception>(() => brush.Convert(_converter));
		}

		[Test]
		[Category("SpeedTest")]
		[Ignore]
		public void LoadTest()
		{
			var sw = new Stopwatch();
			sw.Start();
			for(int i = 0; i < 1000000; i++)
			{
				var brush = new SolidColorBrush
				            	{
				            		Color = GetRandomColor()
				            	};
				ExtendedMaterial res = brush.Convert(_converter);
				CheckColorsEqual(brush.Color, res.MaterialD3D.Diffuse);
			}
			sw.Stop();
			Console.WriteLine(sw.ElapsedMilliseconds);
		}

		public static void CheckMaterial(Color color, Material material)
		{
			CheckColorsEqual(color, material.Diffuse);
			CheckColorsEqual(color, material.Ambient);
		}

		private Color GetRandomColor()
		{
			return Color.FromArgb(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255));
		}

		public static void CheckColorsEqual(Color color, Color4 color4)
		{
			Assert.AreEqual(color.A, color4.Alpha * 255);
			Assert.AreEqual(color.R, color4.Red * 255);
			Assert.AreEqual(color.G, color4.Green * 255);
			Assert.AreEqual(color.B, color4.Blue * 255);
		}

		private readonly Random _random = new Random();
		private DirectXBrushConverter _converter;
		private DeviceWorker _deviceWorker;

		private class FailBrush : IBrush
		{
		}
	}
}