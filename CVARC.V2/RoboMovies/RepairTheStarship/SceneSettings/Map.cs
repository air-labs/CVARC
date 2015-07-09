using System.Collections.Generic;
using System.Linq;

namespace RepairTheStarship.Sensors
{
    public class SettingsMap
    {
        private WallSettings[,] _wallsV;
        private WallSettings[,] _wallsH;
        public List<Node> Nodes { get; private set; }
        public Node this[int x, int y]
        {
            get { return Nodes.FirstOrDefault(a => a.X == x && a.Y == y); }
        }
        public void Init(SceneSettings settings)
        {
            Nodes = new List<Node>();
            _wallsV = settings.VerticalWalls;
            _wallsH = settings.HorizontalWalls;
            Nodes.Clear();
            Add(new Node(0, 0));
        }

        private void GenerateMap(Node currentNode)
        {
            int i = currentNode.X, j = currentNode.Y;
            bool right = _wallsV.GetLength(0) > i && _wallsV.GetLength(1) > j && _wallsV[i, j] == WallSettings.NoWall;
            bool nextRight = _wallsV.GetLength(0) > i+1 && _wallsV.GetLength(1) > j && _wallsV[i+1, j] == WallSettings.NoWall;
            bool left = 0 < i && _wallsV.GetLength(1) > j && _wallsV[i-1, j] == WallSettings.NoWall;
            bool nextLeft = 0 < i-1 && _wallsV.GetLength(1) > j && _wallsV[i-2, j] == WallSettings.NoWall;
            bool bottom = _wallsH.GetLength(0) > i  && _wallsH.GetLength(1) > j && _wallsH[i, j] == WallSettings.NoWall;
            bool nextBottom = _wallsH.GetLength(0) > i  && _wallsH.GetLength(1) > j+1 && _wallsH[i, j+1] == WallSettings.NoWall;
            bool top = _wallsH.GetLength(0) > i && 0 < j && _wallsH[i, j-1] == WallSettings.NoWall;
            bool nextTop = _wallsH.GetLength(0) > i && 0 < j - 1 && _wallsH[i, j - 2] == WallSettings.NoWall;
            if (left)
            {
                var @new = Add(new Node(i - 1, j));
                currentNode.Link(@new);
            }
            if (right)
            {
                var @new = Add(new Node(i + 1, j));
                currentNode.Link(@new);
            }
            if (top)
            {
                var @new = Add(new Node(i, j-1));
                currentNode.Link(@new);
            }
            if (bottom)
            {
                var @new = Add(new Node(i, j+1));
                currentNode.Link(@new);
            }
            return;
            if (left && top && nextLeft && nextTop)
            {
                var @new = Add(new Node(i - 1, j - 1));
                currentNode.Link(@new);
            }
            if (left && bottom && nextLeft && nextBottom)
            {
                var @new = Add(new Node(i - 1, j + 1));
                currentNode.Link(@new);
            }
            if (right && top && nextRight && nextTop)
            {
                var @new = Add(new Node(i + 1, j - 1));
                currentNode.Link(@new);
            }
            if (right && bottom && nextRight && nextBottom)
            {
                var @new = Add(new Node(i + 1, j + 1));
                currentNode.Link(@new);
            }
        }
        public List<Node> FindPath(int fromX, int fromY, int toX, int toY)
        {
            var from = Nodes.FirstOrDefault(a => a.X == fromX && a.Y == fromY);
            var to = Nodes.FirstOrDefault(a => a.X == toX && a.Y == toY);
            return FindPath(from, to, new List<Node>());
        }
        private List<Node> FindPath(Node from, Node to, List<Node> visited)
        {
            if (from.Neighbors.Contains(to))
                return new List<Node>{from, to};
            var lst = new List<Node>{from}.Concat(
                from.Neighbors.Where(a => !visited.Contains(a)).Select(a =>
                                                                   FindPath(a, to,
                                                                            visited.Select(b => b)
                                                                                   .Concat(new[] {from})
                                                                                   .ToList()))
                .Where(a => a.Last() == to)
                .OrderBy(a => a.Count).FirstOrDefault()??new List<Node>()).ToList();
            return lst;
        }
        private Node Add(Node node)
        {
            var was = Nodes.FirstOrDefault(a => a.X == node.X && a.Y == node.Y);
            if (was == null)
            {
                was = node;
                Nodes.Add(was);
                GenerateMap(was);
            }
            return was;
        }

        public bool IsValid()
        {
            return Nodes.Count == _wallsH.GetLength(0)*_wallsV.GetLength(1);
        }
    }
}
