using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Eurosim.Core;
using Eurosim.Graphics.Properties;
using NUnit.Framework;

namespace Eurosim.Graphics.Winforms.Tests
{
	internal class WinformsDrawingTests
	{
		[SetUp]
		public void SetUp()
		{
			var t = new Thread(() =>
				{
					_form = new Form
					        	{
					        		ClientSize = new Size(800, 600)
					        	};
					_pb = new PictureBox {ClientSize = _form.ClientSize};
					_form.Controls.Add(_pb);
					_sync.Set();
					Application.Run(_form);
				});
			t.SetApartmentState(ApartmentState.STA);
			t.Start();
			_sync.WaitOne();
		}

		[Test]
		[Category("SpeedTest")]
		public void ImageDrawingLoadTest()
		{
			_pb.Paint += FormPaint;
			var oldImage = Resources.testtexture;
			_image = Resize(oldImage, oldImage.Width * 2, oldImage.Height * 2);
			for(int i = 0; i < 1000; i++)
			{
				_form.EasyInvoke(x => x.Invalidate(true));
				_sync.WaitOne();
			}
			//Without resize: 2200-2300
			Console.WriteLine("Elapsed {0} ", _sw.ElapsedMilliseconds);
			Assert.Less(_sw.ElapsedMilliseconds, 1000);
		}

		public Image Resize(Image input, int width, int height)
		{
			var newImage = new Bitmap(width, height);
			System.Drawing.Graphics.FromImage(newImage).DrawImage(input, 0, 0, width, height);
			return newImage;
		}

		private void FormPaint(object sender, PaintEventArgs e)
		{
			_sw.Start();
			e.Graphics.DrawImage(_image, 0, 0, _image.Width, _image.Height);
			_sw.Stop();
			_sync.Set();
		}

		private Form _form;
		private readonly AutoResetEvent _sync = new AutoResetEvent(false);
		private PictureBox _pb;
		private readonly Stopwatch _sw = new Stopwatch();
		private Image _image;
	}
}