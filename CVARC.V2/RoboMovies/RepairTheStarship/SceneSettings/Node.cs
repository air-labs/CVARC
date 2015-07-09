using System.Collections.Generic;
using System.Linq;

namespace RepairTheStarship
{
    public class Node
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public List<Node> Neighbors { get; private set; }
        public Node(int x, int y)
        {
            X = x;
            Y = y;
            Neighbors = new List<Node>();
        }
        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
        internal void Link(Node with)
        {
            if (!Neighbors.Any(a => a.X == with.X && a.Y == with.Y))
                Neighbors.Add(with);
            if (!with.Neighbors.Any(a => a.X == this.X && a.Y == this.Y))
                with.Link(this);
        }
    }
}