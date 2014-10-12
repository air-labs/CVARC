using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using CVARC.Core;

namespace CVARC.Graphics
{
	public static class Extensions
	{
		public static bool IsInvisible(this Box box)
		{
			return Math.Abs(box.Volume) < 0.001 ||
			         (box.DefaultColor.RgbEquals(Color.Transparent) &&
			         box.Top == null && box.Bottom == null && box.Front == null &&
			         box.Back == null && box.Right == null && box.Left == null);

		}
		public static bool IsInvisible(this Ball ball)
		{
			return Math.Abs(ball.Radius - 0) < 0.001 || ball.DefaultColor.RgbEquals(Color.Transparent);
		}
		public static bool IsInvisible(this Cylinder cylinder)
		{
			return Math.Abs(cylinder.Volume) < 0.001 ||
			       (cylinder.Top == null &&
			        cylinder.Bottom == null &&
			        cylinder.DefaultColor.RgbEquals(Color.Transparent));
		}

		public static Stream ToStream(this Image image, ImageFormat format)
		{
			var stream = new MemoryStream();
			image.Save(stream, format);
			stream.Position = 0;
			return stream;
		}

		public static void EasyInvoke<T>(this T control, Action<T> action)
			where T : Control
		{
		    if (control.InvokeRequired)
		    {
		        try
		        {
		            control.Invoke(new Action(() => action(control)));
		        }
		        catch (Exception e)
		        {
		            Environment.Exit(1);
		        }
		    }
		    else
		        action(control);
		}
	}
}
