using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RoboMovies
{
    public enum WallSettings
    {
        NoWall,
        Wall,
        RedSocket,
        GreenSocket,
        BlueSocket,
    }

    public enum SideColor
    {
        Green,
        Yellow,
        Any,
    }

    public enum Cinema
    {
        None,
        UpperLeft,
        BottomLeft,
        UpperRight,
        BottomRight,
        LeftStartingArea,
        RightStartingArea,
    }

    public enum ObjectType
    {
        Clapperboard,
        Stand,
        Stairs,
        Light,
        PopCorn,
        Dispenser,
    }

    public struct RMObject
    {
        public readonly SideColor Color;
        public readonly ObjectType Type;

        public RMObject(SideColor color, ObjectType type)
        {
            Color = color; 
            Type = type;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RMObject)) return false;
            var rmobj = (RMObject)obj;
            return rmobj.Color == this.Color && rmobj.Type == this.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (int)Color * 1025 ^ (int)Type;
            }
        }
    }

    public enum DetailColor
    {
        Green,
        Red,
        Blue
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
