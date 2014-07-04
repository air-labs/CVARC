using System.Collections.Generic;
using CVARC.Core;

namespace CVARC.Graphics.DirectX
{
	public class BodyComparer : IEqualityComparer<Body>
	{
		public bool Equals(Body body, Body other)
		{
			if(ReferenceEquals(null, other)) return false;
			if(ReferenceEquals(body, other)) return true;
			if (body is Box)
				return _boxComparer.Equals((body as Box),other);
			if (body is Cylinder)
				return _cylnderComparer.Equals((body as Cylinder), other);
			if (body is Ball)
				return _ballComparer.Equals((body as Ball), other);
			var res = other.DefaultColor.RgbEquals(body.DefaultColor) &&
			Equals(other.Model, body.Model);
			return res;
		}

		public int GetHashCode(Body body)
		{
			unchecked
			{

				var box = body as Box;
				if(box != null)
					return _boxComparer.GetHashCode(box);
				var ball = body as Ball;
				if(ball != null)
					return _ballComparer.GetHashCode(ball);
				var cyl = body as Cylinder;
				if(cyl != null)
					return _cylnderComparer.GetHashCode(cyl);
				return ((body.DefaultColor.GetHashCode()) * 397) ^
				       (body.Model != null ? body.Model.GetHashCode() : 0);
			}
		}
		private readonly BoxComparer _boxComparer=new BoxComparer();
		private readonly BallComparer _ballComparer=new BallComparer();
		private readonly CylnderComparer _cylnderComparer=new CylnderComparer();
	}

	internal class BoxComparer : IEqualityComparer<Box>
	{
		public bool Equals(Box box, Box other)
		{
			if(ReferenceEquals(null, other)) return false;
			if(ReferenceEquals(box, other)) return true;
			return Equals(other.Top, box.Top)
			       && Equals(other.Bottom, box.Bottom)
			       && Equals(other.Right, box.Right) && 
				   Equals(other.Model, box.Model) &&
			       Equals(other.Left, box.Left) &&
			       Equals(other.Back, box.Back) &&
			       other.XSize.Equals(box.XSize) &&
			       other.YSize.Equals(box.YSize) &&
			       other.ZSize.Equals(box.ZSize) &&
			       Equals(other.Front, box.Front);
		}
		public bool Equals(Box box, object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(box, obj)) return true;
			if (obj.GetType() != typeof(Box)) return false;
			return Equals(box,(Box)obj);
		}

		public int GetHashCode(Box box)
		{
			unchecked
			{
				int result = (box.Top != null ? box.Top.GetHashCode() : 0);
				result = (result * 397) ^ (box.Bottom != null ? box.Bottom.GetHashCode() : 0);
				result = (result * 397) ^ (box.Right != null ? box.Right.GetHashCode() : 0);
				result = (result * 397) ^ (box.Left != null ? box.Left.GetHashCode() : 0);
				result = (result * 397) ^ (box.Back != null ? box.Back.GetHashCode() : 0);
				result = (result * 397) ^ (box.Model != null ? box.Model.GetHashCode() : 0);
				result = (result * 397) ^ box.XSize.GetHashCode();
				result = (result * 397) ^ box.YSize.GetHashCode();
				result = (result * 397) ^ box.ZSize.GetHashCode();
				result = (result * 397) ^ (box.Front != null ? box.Front.GetHashCode() : 0);
				return result;
			}
		}
	}

	internal class BallComparer:IEqualityComparer<Ball>
	{
		public bool Equals(Ball ball, Ball other)
		{
			if(ReferenceEquals(null, other)) return false;
			if(ReferenceEquals(ball, other)) return true;
			return other.Radius.Equals(ball.Radius)
			       && other.DefaultColor.RgbEquals(ball.DefaultColor)
			       && Equals(other.Model, ball.Model);
		}

		public bool Equals(Ball ball, object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(ball, obj)) return true;
			if (obj.GetType() != typeof(Ball)) return false;
			return Equals(ball, (Ball)obj);
		}

		public int GetHashCode(Ball ball)
		{
			var result= ball.Radius.GetHashCode();
			result = (result * 397) ^ (ball.Model != null ? ball.Model.GetHashCode() : 0);
			result = (result * 397) ^ (ball.DefaultColor.GetHashCode());
			return result;
		}
	}

	internal class CylnderComparer:IEqualityComparer<Cylinder>
	{
		public bool Equals(Cylinder cylinder, Cylinder other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(cylinder, other)) return true;
			return Equals(other.Top, cylinder.Top) 
				&& Equals(other.Bottom, cylinder.Bottom) 
				&& other.DefaultColor.RgbEquals(cylinder.DefaultColor) 
				&& Equals(other.Model, cylinder.Model) 
				&& other.RTop.Equals(cylinder.RTop) 
				&& other.RBottom.Equals(cylinder.RBottom) 
				&& other.Height.Equals(cylinder.Height);
		}
		public bool Equals(Cylinder cyl, object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(cyl, obj)) return true;
			if (obj.GetType() != typeof(Cylinder)) return false;
			return Equals(cyl, (Cylinder)obj);
		}

		public int GetHashCode(Cylinder cylinder)
		{
			unchecked
			{
				int result = (cylinder.Top != null ? cylinder.Top.GetHashCode() : 0);
				result = (result * 397) ^ (cylinder.Bottom != null ? cylinder.Bottom.GetHashCode() : 0);
				result = (result * 397) ^ cylinder.RTop.GetHashCode();
				result = (result * 397) ^ cylinder.RBottom.GetHashCode();
				result = (result * 397) ^ cylinder.Height.GetHashCode();
				result = (result * 397) ^ (cylinder.Model != null ? cylinder.Model.GetHashCode() : 0);
				result = (result * 397) ^ (cylinder.DefaultColor.GetHashCode());
				return result;
			}
		}
	}
}