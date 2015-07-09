using System;
using System.Collections.Generic;
using CVARC.V2;

namespace RepairTheStarship.MapBuilder
{
    public class PathSearcher
    {
        private static Dictionary<InternalPoint, InternalPoint> parents;
        private static HashSet<InternalPoint> handled;
        private static Queue<InternalPoint> queue;

        public static Direction[] FindPath(InternalMap map, Point from, Point to)
        {
            var lastDirection = Bfs(map, from, to);
            var directions = new List<Direction>();
            var currentPoint = new InternalPoint(to.X, to.Y, lastDirection);
            while (parents.ContainsKey(currentPoint))
            {
                directions.Add(currentPoint.Direction);
                currentPoint = parents[currentPoint];
            }
            directions.Reverse();
            return directions.ToArray();
        }

        private static Direction Bfs(InternalMap map, Point from, Point to)
        {
            queue = new Queue<InternalPoint>();
            parents = new Dictionary<InternalPoint, InternalPoint>();
            handled = new HashSet<InternalPoint>();
            AddPoint(new InternalPoint(from.X, from.Y, Direction.No));

            while (queue.Count > 0)
            {
                var position = queue.Dequeue();
                if (position.X == to.X && position.Y == to.Y)
                    return position.Direction;
                TryAddPosition(map, Direction.Down, position, 0, 1);
                TryAddPosition(map, Direction.Up, position, 0, -1);
                TryAddPosition(map, Direction.Left, position, -1, 0);
                TryAddPosition(map, Direction.Right, position, 1, 0);
            }
            throw new Exception("Path not found.");
        }

        private static void AddPoint(InternalPoint point)
        {
            queue.Enqueue(point);
            handled.Add(point);
        }

        private static void TryAddPosition(InternalMap map, Direction direction, InternalPoint position, int xOffset, int yOffset)
        {
            var availableDirections = map.AvailableDirectionsByCoordinates[position.X, position.Y];
            var point = new InternalPoint(position.X + xOffset, position.Y + yOffset, direction);
            if ( (availableDirections & direction)!= 0 && !handled.Contains(point))
            {
                AddPoint(point);
                parents.SafeAdd(point, position);
            }
        }
    }

    class InternalPoint 
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Direction { get; set; }

        public InternalPoint(int x, int y, Direction direction)
        {
            Direction = direction;
            X = x;
            Y = y;
        }

        protected bool Equals(InternalPoint other)
        {
            return X == other.X && Y == other.Y;
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}, Direction: {2}", X, Y, Direction);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((InternalPoint) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X*397) ^ Y;
            }
        }
    }
}
