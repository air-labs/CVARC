using System;

namespace RepairTheStarship.MapBuilder
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

    public static class DirectionHelper
    {
        public static int ToAngle(this Direction direction)
        {
            if (direction == Direction.Up)
                return -270;
            if (direction == Direction.Right)
                return 0;
            if (direction == Direction.Down)
                return -90;
            if (direction == Direction.Left)
                return -180;
            throw new ArgumentOutOfRangeException();
        }

        public static Direction Invert(this Direction direction)
        {
            if (direction == Direction.Up)
                return Direction.Down;
            if (direction == Direction.Right)
                return Direction.Left;
            if (direction == Direction.Down)
                return Direction.Up;
            if (direction == Direction.Left)
                return Direction.Right;
            throw new ArgumentOutOfRangeException();
        }
    }
}