using System.Collections.Generic;
using System.Linq;
using RepairTheStarship.Sensors;

namespace MapHelper
{
    public class MapBuilder
    {
        private const int MapWidth = 300;
        private const int MapHeight = 200;
        private const int CellSize = 50;

        public Map BuildStaticMap(IEnumerable<MapItem> mapItems)
        {
            var bitArray = CreateArray(MapWidth / CellSize + 2, MapHeight / CellSize + 2);
            var originalWalls = mapItems.Where(x => x.Tag.Contains("Socket") || x.Tag.Contains("Wall"));
            List<Wall> walls = new List<Wall>();
            foreach (var originalWall in originalWalls)
            {
                int x = (int)((originalWall.X + MapWidth / 2) / CellSize) + 1;
                int y = (int)((originalWall.Y - MapHeight / 2) / CellSize) * -1 + 1;
                var wall = new Wall(x, y, originalWall.Tag, originalWall.X, originalWall.Y);
                if (originalWall.Tag.Contains("Vertical"))
                {
                    bitArray[x, y] &= ~Direction.Left;
                    bitArray[x - 1, y] &= ~Direction.Right;
                }
                else if (originalWall.Tag.Contains("Horizontal"))
                {
                    bitArray[x, y] &= ~Direction.Up;
                    bitArray[x, y - 1] &= ~Direction.Down;
                }
                walls.Add(wall);
            }
            return new Map
            {
                BitArray = bitArray,
                Walls = walls
            };
        }

        private Direction[,] CreateArray(int x, int y)
        {
            var bitArray = new Direction[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                {
                    bitArray[i, j] = Direction.All;
                    var isBorder = i == 0 || j == 0 || i == x - 1 || j == y - 1;
                    if (isBorder)
                    {
                        bitArray[i, j] = Direction.No;
                        continue;
                    }
                    if (i == 1)
                        bitArray[i, j] &= ~Direction.Left;
                    if (i == x - 2)
                        bitArray[i, j] &= ~Direction.Right;
                    if (j == 1)
                        bitArray[i, j] &= ~Direction.Up;
                    if (j == y - 2)
                        bitArray[i, j] &= ~Direction.Down;
                }
            return bitArray;
        }
    }
}
