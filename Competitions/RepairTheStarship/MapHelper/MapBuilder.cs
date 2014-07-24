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

        public Map BuildStaticMap(SensorsData sensorsData)
        {
            var availableDirections = RemoveDirectionsContactingWithBorder(MapWidth / CellSize + 2, MapHeight / CellSize + 2);
            var walls = GetWallsWithDiscreteCoordinates(sensorsData.MapSensor.MapItems);
            UpdateAvailableDirections(walls, availableDirections);
            return new Map(sensorsData)
            {
                AvailableDirectionsByCoordinates = availableDirections,
                Walls = walls,
            };
        }

        private void UpdateAvailableDirections(Wall[] walls, Direction[,] availableDirections)
        {
            foreach (var wall in walls)
            {
                if (wall.Type.Contains("Vertical"))
                {
                    availableDirections[wall.X, wall.Y] &= ~Direction.Left;
                    availableDirections[wall.X - 1, wall.Y] &= ~Direction.Right;
                }
                else if (wall.Type.Contains("Horizontal"))
                {
                    availableDirections[wall.X, wall.Y] &= ~Direction.Up;
                    availableDirections[wall.X, wall.Y - 1] &= ~Direction.Down;
                }
            }
        }

        private Wall[] GetWallsWithDiscreteCoordinates(IEnumerable<MapItem> mapItems)
        {
            return mapItems.Where(IsWall).Select(w =>
            {
                int x = (int)((w.X + MapWidth / 2) / CellSize) + 1;
                int y = (int)((w.Y - MapHeight / 2) / CellSize) * -1 + 1;
                return new Wall(x, y, w.Tag, w.X, w.Y);
            }).ToArray();
        }

        private bool IsWall(MapItem mapItem)
        {
            return mapItem.Tag.Contains("Socket") || mapItem.Tag.Contains("Wall");
        }

        private Direction[,] RemoveDirectionsContactingWithBorder(int x, int y)
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
