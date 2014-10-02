using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using CVARC.Core;
using NUnit.Framework;

namespace CVARC.Graphics.DirectX
{
	internal class TwoSceneTests
	{
		[SetUp]
		public void SetUp()
		{
			_sceneRoots.Add(new Ball
				                 	{
				                 		Radius = 10,
				                 		DefaultColor= Color.Yellow
				                 	});
			_sceneRoots.Add(new Box
				                 	{
				                 		XSize = 10,
										YSize = 10,
										ZSize = 10, 
										DefaultColor=Color.Red
				                 	});
			foreach(var scene in _sceneRoots)
			{
				var drawerControl = CreateDrawerControl(scene);
				_forms.Add(drawerControl.TopLevelForm);
			}
		}

		private static DrawerControl CreateDrawerControl(Body scene)
		{
			return new DrawerFactory(scene).CreateAndRunDrawerInStandaloneForm(VideoModes.DirectX);
		}

		[TearDown]
		public void TearDown()
		{
			foreach(var form in _forms)
			{
				if (!form.IsDisposed)
					form.EasyInvoke(x=>x.Dispose());
			}
			Thread.Sleep(500);
		}

		[Test]
		public void Test1()
		{
			Thread.Sleep(1000);
			Assert.Pass();
		}

		private readonly List<Body> _sceneRoots = new List<Body>();
		private readonly List<Form> _forms=new List<Form>();
	}
}