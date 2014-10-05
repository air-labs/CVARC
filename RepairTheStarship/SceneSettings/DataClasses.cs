using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RepairTheStarship
{
    public enum WallSettings
    {
        NoWall,
        Wall,
        RedSocket,
        GreenSocket,
        BlueSocket,
    }

    public enum DetailColor
    {
        Red,
        Green,
        Blue,
    }

    public struct DetailData
    {
        public DetailColor Color { get; set; }
        public Point Location { get; set; }
    }

    public enum WallOrientation
    {
        Horizontal,
        Vertical,
    }

    public struct WallData
    {
        public WallOrientation Orientation { get; set; }
        public WallSettings Type { get; set; }
        public override bool Equals(object obj)
        {
            if (!(obj is WallData)) return false;
            var d = (WallData)obj;
            return Orientation == d.Orientation && Type == d.Type;
        }
        public override int GetHashCode()
        {
            return Orientation.GetHashCode() ^ Type.GetHashCode();
        }

        public bool Match(DetailColor detailColor)
        {
            if (Type == WallSettings.BlueSocket && detailColor == DetailColor.Blue) return true;
            if (Type == WallSettings.RedSocket && detailColor == DetailColor.Red) return true;
            if (Type == WallSettings.GreenSocket && detailColor == DetailColor.Green) return true;
            return false;
        }
    }
}
