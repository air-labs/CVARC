using System;
using System.Drawing;
using System.Collections.Generic;

namespace CVARC.Core
{
	[Serializable]
	public class Cylinder : Body
	{
		public override void AcceptVisitor(IBodyVisitor visitor)
		{
			visitor.Visit(this);
		}

		public override string ToString()
		{
			return string.Format("RTop: {0}, RBottom: {1}, Height: {2}", _rTop, _rBottom, _height);
		}

		public override Color DefaultColor
		{
			get { return base.DefaultColor; }
			set
			{
				base.DefaultColor = value;
				Top = new SolidColorBrush {Color = value};
				Bottom = new SolidColorBrush {Color = value};
				Side = new SolidColorBrush {Color = value};
			}
		}

		public override double Volume { get { return 1.0 / 3.0 * Math.PI * Height * (RTop * RTop + RTop * RBottom + RBottom * RBottom); } }

		public double RTop
		{
			get { return _rTop; }
			set
			{
				CheckGreaterOrZero(value);
				SetField(ref _rTop, value, TopRadiusPropertyName);
			}
		}

		public double RBottom
		{
			get { return _rBottom; }
			set
			{
				CheckGreaterOrZero(value);
				SetField(ref _rBottom, value, BottomRadiusPropertyName);
			}
		}

		public double Height
		{
			get { return _height; }
			set
			{
				CheckGreaterOrZero(value);
				SetField(ref _height, value, HeightPropertyName);
			}
		}

        protected override double dZ()
        {
            return _height / 2;
        }
        
        protected override double dPitchGrad()
        {
            return 90;
        }

        public override List<object> getCreateJSON()
        {
            List<object> result = base.getEditJSON();
            result.Insert(0, -3);
            result.Add(_rTop);
            result.Add(_rBottom);
            result.Add(_height);
            return result;
        }

		public IPlaneBrush Top { get { return _top; } set { SetField(ref _top, value, TopBrushPropertyName); } }

		public IPlaneBrush Bottom { get { return _bottom; } set { SetField(ref _bottom, value, BottomBrushPropertyName); } }

		public SolidColorBrush Side { get { return _side; } set { SetField(ref _side, value, SideBrushPropertyName); } }

		public const string SideBrushPropertyName = "CylinderSideBrush";
		public const string BottomBrushPropertyName = "CylinderBottomBrush";
		public const string TopBrushPropertyName = "CylinderTopBrush";
		public const string BottomRadiusPropertyName = "BottomRadius";
		public const string TopRadiusPropertyName = "TopRadius";
		private IPlaneBrush _top;
		private IPlaneBrush _bottom;
		private SolidColorBrush _side;
		private double _rTop;
		private double _rBottom;
		private double _height;
		private const string HeightPropertyName = "Height";
	}
}