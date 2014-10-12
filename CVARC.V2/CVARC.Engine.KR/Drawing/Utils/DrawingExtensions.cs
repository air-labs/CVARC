using System.Drawing;
using Eurosim.Core;

namespace Eurosim.Graphics
{
	public static class DrawingExtensions
	{
		public static bool IsEmpty(this Box box)
		{
			return !(box.XSize > 0 && box.YSize > 0 && box.ZSize > 0) ||
			         (box.DefaultColor == Color.Transparent &&
			         box.Top == null && box.Bottom == null && box.Front == null &&
			         box.Back == null && box.Right == null && box.Left == null);

		}
		public static bool IsEmpty(this Ball ball)
		{
			return !(ball.Radius > 0) || ball.DefaultColor == Color.Transparent;
		}
		public static bool IsEmpty(this Cylinder cylinder)
		{
			return !(cylinder.Height > 0 && cylinder.RBottom > 0 || cylinder.RTop > 0) || (
				   cylinder.Top == null && cylinder.Bottom == null && cylinder.DefaultColor == Color.Transparent);
		}

		public static bool TryGetColor(this IBrush brush, out Color color)
		{
			var solidColorBrush = (brush as SolidColorBrush);
			color = default(Color);
			if (solidColorBrush != null)
			{
				color = solidColorBrush.Color;
				return true;
			}
			return false;
		}
	}
}
