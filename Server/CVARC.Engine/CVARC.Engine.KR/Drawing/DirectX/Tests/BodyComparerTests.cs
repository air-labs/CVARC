using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CVARC.Core;
using CVARC.Engine.KR.Properties;
using NUnit.Framework;

namespace CVARC.Graphics.DirectX
{
	[TestFixture]
	public class GraphicsBodyComparerTests
	{
		[TestCaseSource("ModelEqulityTestCaseSource")]
		public bool TestModelEquality(Model m1, Model m2)
		{
			bool res = m1.Equals(m2);
			return res;
		}

		[TestCaseSource("ImageBrushEqulityTestCaseSource")]
		public bool TestImageBrushEquality(PlaneImageBrush m1,
		                                   PlaneImageBrush m2)
		{
			bool res = m1.Equals(m2);
			return res;
		}

		[Test]
		public void Equal()
		{
			var x1 = new Ball
			         	{
			         		Radius = 3,
			         		DefaultColor =  Color.Yellow,
			         		Model = new Model {FilePath = "queen"}
			         	};
			var x2 = new Ball
			         	{
			         		Radius = 3,
			         		DefaultColor = Color.FromArgb(255, 255, 255, 0),
			         		Model = new Model {FilePath = "queen"}
			         	};
			var x3 = new Ball
			         	{
			         		Radius = 3,
			         		DefaultColor = Color.Yellow,
			         		Model = new Model {FilePath = "queen"}
			         	};
			Assert.That(_comparer.Equals(x1, x2));
			Assert.That(_comparer.Equals(x2, x3));
		}

		[Test]
		public void TestBoxes()
		{
			var box1 = new Box
			           	{
			           		DefaultColor = Color.Red
			           	};
			var box2 = new Box
			           	{
			           		Top = new SolidColorBrush {Color = Color.Red},
			           		Bottom = new SolidColorBrush {Color = Color.Red},
			           		Front = new SolidColorBrush {Color = Color.Red},
			           		Back = new SolidColorBrush {Color = Color.Red},
			           		Right = new SolidColorBrush {Color = Color.Red},
			           		Left = new SolidColorBrush {Color = Color.Red}
			           	};
			Assert.That(_comparer.Equals(box1, box2));
			box2.Front = new PlaneImageBrush {FilePath = "aax"};
			Assert.That(!_comparer.Equals(box1, box2));
			box2.Front = new SolidColorBrush {Color = box1.DefaultColor};
			Assert.That(_comparer.Equals(box1, box2));
			box2.Model = new Model();
			Assert.That(!_comparer.Equals(box1, box2));
		}

		[Test]
		public void NotEqual()
		{
			var x1 = new Ball
			         	{
			         		Radius = 3,
			         		DefaultColor = Color.Yellow,
			         		Model = new Model {FilePath = "queen"}
			         	};
			var x2 = new Ball
			         	{
			         		Radius = 3,
			         		DefaultColor = Color.Green,
			         		Model = new Model {FilePath = "queen"}
			         	};
			var x3 = new Ball
			         	{
			         		Radius = 3,
			         		DefaultColor = Color.Yellow,
			         		Model = new Model {FilePath = "king"}
			         	};
			Assert.That(!_comparer.Equals(x1, x2));
			Assert.That(!_comparer.Equals(x2, x3));
			Assert.That(!_comparer.Equals(x1, x3));
		}

		public static IEnumerable<TestCaseData> ModelEqulityTestCaseSource
		{
			get
			{
				return new[]
				       	{
				       		new TestCaseData(
				       			Model.FromResource(()=>Resources.queenModel),
				       			Model.FromResource(()=>Resources.queenModel))
				       			.Returns(true),
				       		new TestCaseData(
				       			new Model {FilePath = GetTestFilePath()},
				       			new Model {FilePath = GetTestFilePath()})
				       			.Returns(true),
				       		new TestCaseData(
				       			new Model {FilePath = GetTestFilePath()},
				       			new Model {FilePath = "abc"})
				       			.Returns(false)
				       	};
			}
		}

		public IEnumerable<TestCaseData> ImageBrushEqulityTestCaseSource
		{
			get
			{
				var bitmap = new Bitmap(10, 10);
				return new[]
				       	{
				       		new TestCaseData(
#pragma warning disable 612,618
				       			new PlaneImageBrush {Image = bitmap},
				       			new PlaneImageBrush {Image = bitmap})
#pragma warning restore 612,618
				       			.Returns(true),
								new TestCaseData(
				       			PlaneImageBrush.FromResource(()=>Resources.testtexture),
				       			PlaneImageBrush.FromResource(()=>Resources.testtexture))
				       			.Returns(true),
				       		new TestCaseData(
				       			new PlaneImageBrush {FilePath = GetTestFilePath()},
				       			new PlaneImageBrush {FilePath = GetTestFilePath()})
				       			.Returns(true),
				       		new TestCaseData(
				       			new PlaneImageBrush {FilePath = GetTestFilePath()},
				       			new PlaneImageBrush {FilePath = "abc"})
				       			.Returns(false)
				       	};
			}
		}

		private static string GetTestFilePath()
		{
			const string tmp = "tmp";
			if(!Directory.Exists(tmp))
				Directory.CreateDirectory(tmp);
			if(_filePath == null)
			{
				_filePath = Path.Combine(tmp, Guid.NewGuid().ToString());
				File.WriteAllText(_filePath, Guid.NewGuid().ToString());
			}
			return _filePath;
		}

		private static string _filePath;

		private readonly BodyComparer _comparer = new BodyComparer();
	}
}