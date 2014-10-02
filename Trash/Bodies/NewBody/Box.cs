using System;
using System.Drawing;
using AIRLab.Mathematics;
using System.Collections.Generic;

namespace CVARC.Core
{
	///Box - это тело, у которого форма ящика
	///К Box по прежнему можно аттачить тела, как и ко всему
	[Serializable]
	public class Box : Body
	{
		public Box()
		{
		}

		public Box(double xSize, double ySize, double zSize)
		{
			XSize = xSize;
			YSize = ySize;
			ZSize = zSize;
		}

		public Box(double xSize, double ySize, double zSize, Color defaultColor) : this(xSize, ySize, zSize)
		{
			DefaultColor = defaultColor;
		}

		public override void AcceptVisitor(IBodyVisitor visitor)
		{
			visitor.Visit(this);
		}

		public override string ToString()
		{
			return string.Format("XSize: {0}, YSize: {1}, ZSize: {2}", _xSize, _ySize, _zSize);
		}

        protected override double dZ() {
            return _zSize / 2;
        }

        public override List<object> getCreateJSON()
        {
            List<object> result = base.getEditJSON();
            result.Insert(0, -2);
            result.Add(_xSize);
            result.Add(_ySize);
            result.Add(_zSize);

            if (_top != null)
                   if(_top is PlaneImageBrush)
                       result.Add((_top as PlaneImageBrush).FilePath);

            return result;
        }

		public override double Volume { get { return XSize * YSize * ZSize; } }

		public double XSize
		{
			get { return _xSize; }
			set
			{
				CheckGreaterOrZero(value);
				SetField(ref _xSize, value, XSizePropertyName);
			}
		}

		public double YSize
		{
			get { return _ySize; }
			set
			{
				CheckGreaterOrZero(value);
				SetField(ref _ySize, value, YSizePropertyName);
			}
		}

		public double ZSize
		{
			get { return _zSize; }
			set
			{
				CheckGreaterOrZero(value);
				SetField(ref _zSize, value, ZSizePropertyName);
			}
		}

		public override Color DefaultColor
		{
			get { return base.DefaultColor; }
			set
			{
				base.DefaultColor = value;
				_top = _top ?? (new SolidColorBrush {Color = value});
				_bottom = _bottom ?? (new SolidColorBrush {Color = value});
				_front = _front ?? (new SolidColorBrush {Color = value});
				_right = _right ?? (new SolidColorBrush {Color = value});
				_left = _left ?? (new SolidColorBrush {Color = value});
				_back = _back ?? (new SolidColorBrush {Color = value});
			}
		}

		/// <summary>
		/// "+" направление по оси OZ
		/// </summary>
		public IPlaneBrush Top { get { return _top; } set { SetField(ref _top, value, TopBrushPropertyName); } }

		/// <summary>
		/// "-" направление по оси OZ
		/// </summary>
		public IPlaneBrush Bottom { get { return _bottom; } set { SetField(ref _bottom, value, BottomBrushPropertyName); } }

		/// <summary>
		/// "+" направление по оси OY
		/// </summary>
		public IPlaneBrush Right { get { return _right; } set { SetField(ref _right, value, RightBrushPropertyName); } }

		/// <summary>
		/// "-" направление по оси OY
		/// </summary>
		public IPlaneBrush Left { get { return _left; } set { SetField(ref _left, value, LeftBrushPropertyName); } }

		/// <summary>
		/// "+" направление по оси OX
		/// </summary>
		public IPlaneBrush Front { get { return _front; } set { SetField(ref _front, value, FrontBrushPropertyName); } }

		/// <summary>
		/// "-" направление по оси OX
		/// </summary>
		public IPlaneBrush Back { get { return _back; } set { SetField(ref _back, value, BackBrushPropertyName); } }

		public const string YSizePropertyName = "YSize";
		public const string XSizePropertyName = "XSize";
		public const string ZSizePropertyName = "ZSize";
		public const string TopBrushPropertyName = "BoxTopBrush";
		public const string BottomBrushPropertyName = "BoxBottomBrush";
		public const string RightBrushPropertyName = "RightBrush";
		public const string LeftBrushPropertyName = "LeftBrush";

		public const string BackBrushPropertyName = "BackBrush";
		public const string FrontBrushPropertyName = "FrontBrush";
		private IPlaneBrush _top;
		private IPlaneBrush _bottom;
		private IPlaneBrush _right;
		private IPlaneBrush _left;
		private IPlaneBrush _front;
		private IPlaneBrush _back;

		private double _xSize;
		private double _ySize;
		private double _zSize;


      
	}

   
}