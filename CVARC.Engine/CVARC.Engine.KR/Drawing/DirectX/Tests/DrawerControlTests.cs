using System.Drawing;
using System.Windows.Forms;
//using Moq;
using NUnit.Framework;
/*
namespace CVARC.Graphics
{
	public partial class DrawerControl
	{
		public class DrawerControlTests
		{
			private FormDrawer _mockedDrawer;
			private DrawerControl _drawerCon;

			[Test]
			public void TestSizeBoundToParent()
			{
				var form = new Form {ClientSize = new Size(800, 600)};
				form.Controls.Add(_drawerCon);
				Assert.AreEqual(form.ClientSize, _drawerCon.ClientSize);
				form.ClientSize = new Size(120, 120);
				Assert.AreEqual(form.ClientSize, _drawerCon.ClientSize);
				var lastFormSize = new Size(800, 700);
				form.ClientSize = lastFormSize;
				Assert.AreEqual(form.ClientSize, _drawerCon.ClientSize);

				form.Controls.Remove(_drawerCon);
				form.ClientSize = new Size(1, 1);
				Assert.AreEqual(lastFormSize, _drawerCon.ClientSize);
			}

			[Test]
			public void TestNullDrawerDoesNotThrow()
			{
				Assert.DoesNotThrow(() =>
					{
						var control = new DrawerControl(null);
						control.ResizeThisToParent(null, null);
						control.OnParentChanged(null);
						control.OnResize(null);
						control.OnHandleCreated(null);
						control.Dispose(false);
					});

			}

			[SetUp]
			public void SetUp()
			{
				_mockedDrawer = Mock.Of<FormDrawer>();
				_drawerCon = new DrawerControl(_mockedDrawer)
					{
						Size = new Size(800, 600)
					};

			}
		}
	}
}
*/