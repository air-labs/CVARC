using System.Windows.Forms;
using NUnit.Framework;
using SlimDX.Direct3D9;
using System.Linq;

namespace CVARC.Graphics.DirectX
{
	internal partial class DeviceWorker
	{
		[TestFixture]
		public class DeviceCreationTests
		{
			[SetUp]
			public void SetUp()
			{
				_d3D = new Direct3D();
				_form = new Form();
				_deviceSettings =
					new DeviceSettings
						{
							PresentParameters = new PresentParameters
								{
									BackBufferWidth = 10,
									BackBufferHeight = 10,
									EnableAutoDepthStencil = false,
									DeviceWindowHandle = _form.Handle,
								}
						};
			}

			[TearDown]
			public void TearDown()
			{
				_d3D.Dispose();
				if(_device != null)
					_device.Dispose();
			}

			[Test]
			public void Software()
			{
				bool result = TryCreateSoftwareDevice(_d3D, _deviceSettings, out _device);
				Assert.That(result);
			}

			[Explicit("Machine-configuration dependent")]
			[Test]
			public void Hardware()
			{
				bool result = TryCreateHardwareDevice(_d3D, _deviceSettings, out _device);
				Assert.That(result);
			}


			[Explicit("Machine-configuration dependent")]
			[Test]
			public void MultisamplingTypes()
			{
				_deviceSettings.AutoDetermineMultisampleType = false;
				foreach(MultisampleType multisampleType in new[] {MultisampleType.None,MultisampleType.FourSamples})
				{
					_deviceSettings.MultisampleType = multisampleType;
					Device device = null;
					try
					{
						bool result = TryCreateHardwareDevice(_d3D, _deviceSettings, out device);
						Assert.That(result);
						using(Surface renderTarget = device.GetRenderTarget(0))
						{
							Assert.AreEqual(renderTarget.Description.MultisampleType, multisampleType);
							Assert.AreEqual(renderTarget.Description.MultisampleQuality, (int)multisampleType);
						}
					}
					finally
					{
						if(device != null) device.Dispose();
					}
				}
			}
			[Test]
			public void MultiSamplingTypeToQuality()
			{
				var qualities = new[] {MultisampleType.None, MultisampleType.TwoSamples, MultisampleType.FourSamples}.Select(x => (int)x);
				CollectionAssert.AreEqual(new[] {0, 2, 4}, qualities);
			}

			private Direct3D _d3D;
			private DeviceSettings _deviceSettings;
			private Form _form;
			private Device _device;
		}
	}
}