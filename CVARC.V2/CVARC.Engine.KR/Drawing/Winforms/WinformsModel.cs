using System;
using System.Drawing;
using CVARC.Core;

namespace CVARC.Graphics.Winforms
{
	public class WinformsModel : IDisposable
	{
		private WinformsModel()
		{
		}

		public static WinformsModel FromColor(Body body, Color color, Size size)
		{
			return new WinformsModel
			       	{
			       		Brush = new SolidBrush(color),
						Size=size,
			       		Body = body
			       	};
		}

		public static WinformsModel FromImage(Body body, PlaneImageBrush brush, Size size)
		{
#pragma warning disable 612,618
			Image image = brush.Image;
#pragma warning restore 612,618
			
			if (!string.IsNullOrEmpty(brush.FilePath))
				image = Image.FromFile(brush.FilePath);
			else if (!string.IsNullOrEmpty(brush.ResourceName))
                image = (Image)CVARC.Engine.KR.Properties.Resources.ResourceManager.GetObject(brush.ResourceName);
			image.RotateFlip(brush.RotateFlipType);
			var m = new WinformsModel
			        	{
			        		Body = body,
			        		HasImage = true,
							Size = size,
			        		Image =image
			        	};
			var newSizeBitmap = new Bitmap(size.Width, size.Height);
			System.Drawing.Graphics.FromImage(newSizeBitmap).
				DrawImage(m.Image, 0, 0, size.Width, size.Height);
			m.Image.Dispose();
			m.Image = newSizeBitmap;
			return m;
		}

		public void Draw(System.Drawing.Graphics graphics)
		{
			if(HasImage)
				DrawUsingImage(graphics);
			else
				DrawUsingShape(graphics);
		}

		public void Dispose()
		{
			if(Image != null)
				Image.Dispose();
			if(Brush != null)
				Brush.Dispose();
		}

		private void DrawUsingImage(System.Drawing.Graphics graphics)
		{
			graphics.DrawImage(Image, -Size.Width / 2, -Size.Height / 2);
		}

		private void DrawUsingShape(System.Drawing.Graphics graphics)
		{
			if(Body is Box)
			{
				graphics.FillRectangle(Brush,
				                       - (float)Size.Width / 2,
				                       - (float)Size.Height / 2,
				                       Size.Width, Size.Height);
			}
			else if(Body is Ball || Body is Cylinder)
			{
				float rad = Size.Width;
				graphics.FillEllipse(Brush,- rad/2, - rad/2, rad, rad);
			}
		}

		internal class WinformsModelFactory : ModelFactory<WinformsModel>
		{
			public override void Visit(Box visitable)
			{
				if(visitable.IsInvisible())
					return;
				var size = new Size((int)visitable.XSize, (int)visitable.YSize);
				if(visitable.Top is PlaneImageBrush)
				{
					InternalResult = FromImage(visitable, (PlaneImageBrush)visitable.Top,
					                           size);
				}
				else if(visitable.Top is SolidColorBrush)
					InternalResult = FromColor(visitable, ((SolidColorBrush)visitable.Top).Color, size);
			}

			public override void Visit(Ball visitable)
			{
				if(visitable.IsInvisible())
					return;
				var diameter = (int)(visitable.Radius * 2);
				InternalResult = FromColor(visitable, visitable.DefaultColor,
				                           new Size(diameter, diameter));
			}

			public override void Visit(Cylinder visitable)
			{
				if(visitable.IsInvisible())
					return;
				if(visitable.Top is PlaneImageBrush)
					throw new NotImplementedException();
				var solidColorBrush = visitable.Top as SolidColorBrush;
				if(solidColorBrush == null)
					return;
				var diameter = (int)(Math.Max(visitable.RTop,visitable.RBottom) * 2);
				InternalResult = FromColor(visitable, solidColorBrush.Color, new Size(diameter, diameter));
			}

			public override void Visit(Body visitable)
			{

			}

		}

		private Size Size { get; set; }
		private bool HasImage { get; set; }
		private Brush Brush { get; set; }
		private Image Image { get; set; }
		private Body Body { get; set; }
	}

}