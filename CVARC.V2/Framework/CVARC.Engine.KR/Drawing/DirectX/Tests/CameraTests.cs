using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using AIRLab.Mathematics;
using CVARC.Core;
using NUnit.Framework;

namespace CVARC.Graphics.DirectX
{
	class CameraTests
	{
		[Test]
		public void ModePlusPlus()
		{
			var camera = new SwitchableCamera(new Box(), new Frame3D(),
			                                  new Form(), new Frame3D(100, 200, 300));
			var expectedModes = typeof(ViewModes).
				GetMembers(BindingFlags.Public | BindingFlags.Static).
				Select(x=>(ViewModes)Enum.Parse(typeof(ViewModes),x.Name))
				.ToList();
			var firstMode = camera.Mode;
			var modeCountAtStart = expectedModes.Count;
			for(int i = 0; i < modeCountAtStart; i++)
			{
				//Console.WriteLine("Mode is {0}",camera.Mode);
				expectedModes.Remove(camera.Mode);
				camera.Mode++;
			}
			Assert.AreEqual(0,expectedModes.Count);
			Assert.AreEqual(firstMode,camera.Mode);

		}

	}
}
