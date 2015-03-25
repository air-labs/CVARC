using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CVARC.Core;
using CVARC.Engine.KR.Properties;
using NUnit.Framework;
using SlimDX.Direct3D9;

namespace CVARC.Graphics.DirectX
{
	internal partial class DirectXModelFactory
	{
		public class ModelFactoryTests
		{
			[SetUp]
			public void SetUp()
			{
				_deviceWorker = new DeviceWorker();
				_device = _deviceWorker.Device;
				_modelFactory = new DirectXModelFactory(_device);
			}

			[TearDown]
			public void TearDown()
			{
				if(_model != null)
					_model.Dispose();
				_deviceWorker.TryDispose();
			}

			[Test]
			public void Ball()
			{
				var ball = new Ball {DefaultColor = Color.Yellow};
				Assert.False(_modelFactory.TryGetResult(ball, out _model));
				ball.Radius = 5;
				Assert.That(_modelFactory.TryGetResult(ball, out _model));
				Assert.AreEqual(1, _model.Mesh.GetMaterials().Length);
			}

			[Test]
			[TestCaseSource("CylinderTestCaseSource")]
			public void TexturedCylinder (Cylinder cylinder, int textureCount)
			{
				Assert.True(_modelFactory.TryGetResult(cylinder, out _model));
				Assert.AreEqual(textureCount, _model.Textures.Count);
				Assert.AreEqual(3, _model.Mesh.GetMaterials().Length);
			}

			[Test]
			public void TestBoxAndTextures()
			{
				Color defaultColor = Color.Yellow;
				var box = new Core.Box
				          	{
				          		XSize = 30,
				          		YSize = 30,
				          		ZSize = 30,
				          		DefaultColor = defaultColor,
#pragma warning disable 612,618
				          		Top = new PlaneImageBrush {Image = Resources.testtexture},
				          		Front = new PlaneImageBrush {Image = Resources.testtexture},
				          		Right = new PlaneImageBrush {Image = Resources.testtexture},
#pragma warning restore 612,618
				          	};
				_modelFactory.TryGetResult(box, out _model);
				Assert.AreEqual(6, _model.Mesh.GetMaterials().Length);
				Assert.AreEqual(3, _model.Textures.Count);
				foreach(ExtendedMaterial mat in _model.Mesh.GetMaterials())
				{
					Console.WriteLine("{0}: {1}", mat.TextureFileName, mat.MaterialD3D.Diffuse);
					
					if(string.IsNullOrEmpty(mat.TextureFileName))
						BrushConverterTests.CheckMaterial(defaultColor, mat.MaterialD3D);
					else BrushConverterTests.CheckMaterial(DirectXBrushConverter.DefaultColor, mat.MaterialD3D);
				}
			}

			[TestCaseSource("EmptyModelTestCaseSource")]
			public void EmptyModel(Body body)
			{
				_modelFactory.TryGetResult(body, out _model);
				Assert.AreEqual(null, _model);
			}

			[TestCaseSource("ModelContentTestCaseSource")]
			public bool GettingModelContent(Model model, byte[] expectedContent)
			{
				byte[] content;
				bool res = TryGetContentFromModel(model, out content);
				Assert.AreEqual(expectedContent, content);
				return res;
			}

			public IEnumerable<TestCaseData> ModelContentTestCaseSource
			{
				get
				{
					return new[]
					       	{
					       		new TestCaseData(new Model(), null).Returns(false),
					       		new TestCaseData(new Model {FilePath = "abc.txt"}, null).Throws(typeof(FileNotFoundException)),
#pragma warning disable 612,618
					       		new TestCaseData(new Model {Content = Resources.arrowred}, Resources.arrowred).Returns(true),
#pragma warning restore 612,618
					       		new TestCaseData(Model.FromResource(() => Resources.arrowred), Resources.arrowred).Returns(true)
					       	};
				}
			}

			public IEnumerable<TestCaseData> CylinderTestCaseSource
			{
				get
				{
					PlaneImageBrush brush = PlaneImageBrush.FromResource(() => Resources.testtexture);
					return new[]
					       	{
					       		new TestCaseData(new Cylinder {DefaultColor = Color.Yellow, RBottom = 1, RTop = 1, Height = 1}, 0),
					       		new TestCaseData(new Cylinder
					       		                 	{
					       		                 		RBottom = 1, RTop = 1, Height = 1,
					       		                 		Top = brush,
					       		                 	}, 1),
					       		new TestCaseData(new Cylinder
					       		                 	{
					       		                 		RBottom = 1, RTop = 1, Height = 1,
					       		                 		Top = brush,
					       		                 		Bottom = brush
					       		                 	}, 2)
					       	};
				}
			}

			public IEnumerable<TestCaseData> EmptyModelTestCaseSource
			{
				get
				{
					return new Body[]
					       	{
					       		new Cylinder {Height = 10, DefaultColor = Color.Transparent},
					       		new Ball {Radius = 5, DefaultColor = Color.Transparent},
					       		new Core.Box {ZSize = 5, DefaultColor = Color.Transparent},
					       		new Core.Box {DefaultColor = Color.Red},
					       		new Ball {DefaultColor = Color.Red},
					       		new Cylinder {DefaultColor = Color.Red}
					       	}.Select(x => new TestCaseData(x));
				}
			}

			private DeviceWorker _deviceWorker;
			private Device _device;
			private DirectXModelFactory _modelFactory;
			private DirectXModel _model;
		}
	}
}