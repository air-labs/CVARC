using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using AIRLab.Mathematics;
using CVARC.Core;
using NUnit.Framework;
using SlimDX;
using SlimDX.Direct3D9;
using System.Linq;

namespace CVARC.Graphics.DirectX
{
	public partial class DirectXFormDrawer
	{
		[TestFixture]
		public class DrawerTests
		{
			[SetUp]
			public void Setup()
			{
				_drawerFactory= new DrawerFactory(_rootBody);
			}

			[TearDown]
			public void TearDown()
			{
				Console.WriteLine("Teardown started...");
				foreach(Form form in _forms)
					form.EasyInvoke(x => x.Close());
				Console.WriteLine("{0} Forms closed", _forms.Count);
				_forms.Clear();
				_drawers.Clear();
				Thread.Sleep(400);
				Console.WriteLine("Teardown completed.");
			}

			[Test]
			public void Test1()
			{
				_rootBody.Add(new Core.Box{DefaultColor = Color.Red,XSize = 10,YSize = 10,ZSize = 100});
				CreateDrawer();
				Thread.Sleep(100);
				var sw = new Stopwatch();
				sw.Start();
				var mesh = _drawers[0]._scene.Models.First().Value.Mesh;


				var ray = new Ray(new Vector3(1, 2, 3), new Vector3(1, 2, 3));
				for(int i = 0; i < 1000000; i++)
				{
					//SlimDX.Direct3D9.Direct3D.
					mesh.Intersects(ray);
				}
				sw.Stop();
				Console.WriteLine(sw.Elapsed);
			}
			[Test]
			public void StaticSize()
			{
				CreateDrawer();
				DirectXFormDrawer drawer = _drawers[0];
				var form = _forms[0];
				int width = form.ClientSize.Width;
				int height = form.ClientSize.Height;
				CheckSwapChainSize(drawer, width, height);
				double aspectRatio = width / (double)height;
				CheckEqual(aspectRatio, drawer.AspectRatio);
				CheckEqual(aspectRatio, drawer._camera.AspectRatio);
			}

			[Test]
			public void OneResize()
			{
				CreateDrawer();
				const int width = 1000;
				const int height = 700;
				DirectXFormDrawer drawer1 = _drawers[0];
				SetFormSize(drawer1, width, height);
				CheckSwapChainSize(drawer1, width, height);
				const double aspectRatio = width / (double)height;
				
				CheckEqual(aspectRatio, drawer1.AspectRatio);
				CheckEqual(aspectRatio, drawer1._camera.AspectRatio);
			}

			[Test]
			public void ResizeTwice()
			{
				CreateDrawer();
				int width = 1000;
				int height = 700;
				SetFormSize(_drawers[0], width, height);
				CheckSwapChainSize(_drawers[0], width, height);
				double aspectRatio = width / (double)height;
				DirectXFormDrawer drawer1 = _drawers[0];
				CheckEqual(aspectRatio, drawer1.AspectRatio);
				CheckEqual(aspectRatio, drawer1._camera.AspectRatio);
				width = 120;
				height = 70;
				SetFormSize(_drawers[0], width, height);
				CheckSwapChainSize(_drawers[0], width, height);
				CheckDeviceSize(_drawers[0], width, height);
				aspectRatio = width / (double)height;
				CheckEqual(aspectRatio, drawer1.AspectRatio);
				CheckEqual(aspectRatio, drawer1._camera.AspectRatio);
			}

			[Test]
			public void TwoDrawers()
			{
				CreateDrawer();
				CreateDrawer();
				var dr0 = _drawers[0];
				var dr1 = _drawers[1];
				SetFormSize(dr0, 900, 600);
				const int smallwidth = 120;
				const int smallheight = 127;
				SetFormSize(dr1, smallwidth, smallheight);
				CheckSwapChainSize(dr1, smallwidth, smallheight);
				
				CheckSwapChainSize(dr0, 900, 600);
				CheckDeviceSize(dr1, 900, 600);
				
				SetFormSize(dr1, 1000, 700);
				CheckSwapChainSize(dr1, 1000, 700);
				CheckSwapChainSize(dr0, 900, 600);
				CheckDeviceSize(dr1, 1000, 700);
				
				const int newSmallSize = 180;
				SetFormSize(dr0, newSmallSize, newSmallSize);
				SetFormSize(dr1, newSmallSize, newSmallSize);
				CheckSwapChainSize(dr1, newSmallSize, newSmallSize);
				CheckSwapChainSize(dr0, newSmallSize, newSmallSize);
				CheckDeviceSize(dr1, newSmallSize, newSmallSize);
				
				Console.WriteLine("Closing one form");
				
				Thread.Sleep(1000);
				_forms[0].EasyInvoke(x => x.Close());
				
				//one remaining now.
				SetFormSize(dr1, 900, 600);
				CheckSwapChainSize(dr1, 900, 600);
				CheckDeviceSize(dr1, 900, 600);
				SetFormSize(dr1, smallwidth, smallheight);
				CheckSwapChainSize(dr1, smallwidth, smallheight);
				CheckDeviceSize(dr1, smallwidth, smallheight);}

			[Test]
			public void FormMinimized()
			{
				CreateDrawer();
				var form = _forms[0];
				int width = form.ClientSize.Width;
				int height = form.ClientSize.Height;
				form.WindowState = FormWindowState.Minimized;
				CheckSwapChainSize(_drawers[0], width, height);
			}

			[Test]
			[Category("SpeedTest")]
			[Ignore]
			public void HardPerformanceTest()
			{
				SceneConfig.Framerate = 1000;
				for(int i = 0; i < 30; i++)
				{
					_rootBody.Add(new Core.Box
					              	{
					              		XSize = 10,
					              		YSize = 20,
					              		ZSize = 30,
					              		DefaultColor = Color.Yellow,
					              		Top = PlaneImageBrush.FromResource(() => Properties.Resources.untitled),
					              		Bottom = PlaneImageBrush.FromResource(() => Properties.Resources.untitled),
					              		Location = GetRandomFrame()
					              	});
					_rootBody.Add(new Body
					              	{
					              		Model = Model.FromResource(() => Properties.Resources.totem),
					              		Location = GetRandomFrame()
					              	});
					_rootBody.Add(new Cylinder
					              	{
										RTop = 20,
										RBottom = 30,
										Height = 10,
					              		DefaultColor = Color.Red,
					              		Location = GetRandomFrame()
					              	});
				}
				CreateDrawer();
				Thread.Sleep(TimeSpan.FromSeconds(20));
			}

			private Frame3D GetRandomFrame()
			{
				return new Frame3D(_random.Next(100), _random.Next(100), _random.Next(100));
			}

			private void CreateDrawer()
			{
				var drawerControl = _drawerFactory.CreateAndRunDrawerInStandaloneForm(VideoModes.DirectX);
				_drawers.Add(drawerControl.Drawer as DirectXFormDrawer);
				_forms.Add(drawerControl.TopLevelForm);
			}

			private static void CheckDeviceSize(DirectXFormDrawer drawer, int width, int height)
			{
				Assert.AreEqual(width, drawer._deviceWorker.DeviceSize.Width);
				Assert.AreEqual(height, drawer._deviceWorker.DeviceSize.Height);
			}

			private static void CheckSwapChainSize(DirectXFormDrawer drawer, int width, int height)
			{
				Assert.AreEqual(width, drawer._swapChain.PresentParameters.BackBufferWidth);
				Assert.AreEqual(height, drawer._swapChain.PresentParameters.BackBufferHeight);
			}

			// ReSharper disable UnusedParameter.Local
			private static void CheckEqual(double expected, double actual)
			{
				Assert.That(Math.Abs(expected - actual) < 0.0001, "Expected {0}, but was {1}", expected, actual);
			}

			// ReSharper restore UnusedParameter.Local

			private void SetFormSize(DirectXFormDrawer drawer, int width, int height)
			{
				_forms[_drawers.IndexOf(drawer)].EasyInvoke(x => x.ClientSize = new Size(width, height));
			}


			private readonly Body _rootBody = new Body
			                                  	{new Ball {Radius = 15, DefaultColor = Color.Red}};

			private readonly List<DirectXFormDrawer> _drawers = new List<DirectXFormDrawer>();
			private readonly Random _random=new Random(12345);
			private readonly List<Form> _forms=new List<Form>();
			private DrawerFactory _drawerFactory;
		}
	}
}