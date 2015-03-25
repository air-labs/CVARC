using System.Drawing;
using AIRLab.Mathematics;
using CVARC.Core;
using NUnit.Framework;
using SlimDX;
using SlimDX.Direct3D9;

namespace CVARC.Graphics.DirectX
{
	[TestFixture]
	internal class LightConversionTests
	{
		[TestFixtureSetUp]
		public void Setup()
		{
			_deviceWorker = new DeviceWorker();
			_device = _deviceWorker.Device;
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			_deviceWorker.TryDispose();
		}

		[Test]
		public void ValidAmbient()
		{
			Light l = new LightSettings
			          {
			          	ColorString = "White",
			          	Type = LightSettings.MyLightType.Ambient,
			          }.ToDirectXLight();
			Assert.That(Color.White.RgbEquals(l.Ambient.ToColor()));
			ValidateWithDevice(l);
		}

		[Test]
		public void ValidColor()
		{
			Light l = new LightSettings
			          {
			          	ColorString = "Red",
			          	Type = LightSettings.MyLightType.Directional,
			          	Direction = new Point3D(0, 0, -1)
			          }.ToDirectXLight();
			Assert.That(Color.Red.RgbEquals(l.Diffuse.ToColor()));
			ValidateWithDevice(l);
		}

		[Test]
		public void Directional()
		{
			var direction = new Point3D(0, 0, -1);
			Light l = new LightSettings
			          {
			          	ColorString = "White",
			          	Type = LightSettings.MyLightType.Directional,
			          	Direction = direction
			          }.ToDirectXLight();
			ValidateWithDevice(l);
		}

		[Test]
		public void Spot()
		{
			var position = new Point3D(200, 300, -100);
			var direction = new Point3D(0, 0, -1);
			Light l = new LightSettings
			          {
			          	ColorString = "White",
			          	Type = LightSettings.MyLightType.Spot,
			          	Position = position,
			          	Direction = direction
			          }.ToDirectXLight();
			ValidateWithDevice(l);
		}

		[Test]
		public void Point()
		{
			var position = new Point3D(200, 300, -100);
			Light l = new LightSettings
			          {
			          	ColorString = "White",
			          	Type = LightSettings.MyLightType.Point,
			          	Position = position,
			          }.ToDirectXLight();
			Assert.AreEqual(position.ToDirectXVector(), l.Position);
			ValidateWithDevice(l);
		}

		private void ValidateWithDevice(Light light)
		{
			Result res = _device.SetLight(0, light);
			Assert.AreEqual(ResultCode.Success, res);
		}

		private DeviceWorker _deviceWorker;
		private Device _device;
	}
}