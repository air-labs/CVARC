using System;

namespace MapHelper
{
    [Flags]
    public enum Direction
    {
        No,
        Down = 1,
        Left = 2,
        Right = 4,
        Up = 8,
        All = Down | Left | Right | Up,
    }
}