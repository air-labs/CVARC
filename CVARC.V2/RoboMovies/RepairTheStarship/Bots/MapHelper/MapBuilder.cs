using RepairTheStarship;

namespace RepairTheStarship.MapBuilder
{
    public static class MapBuilder
    {
        private const int MapWidth = 300;
        private const int MapHeight = 200;
        private const int CellSize = 50;

        public static InternalMap BuildMap(this BotsSensorsData positionSensorsData)
        {
            var map = new InternalMap(positionSensorsData);
            map.AvailableDirectionsByCoordinates = GetAvailableDirections(map.Walls);
            return map;
        }

        private static Direction[,] GetAvailableDirections(StarshipObject[] walls)
        {
            var availableDirections = RemoveDirectionsContactingWithBorder(MapWidth / CellSize + 2, MapHeight / CellSize + 2);
            foreach (var wall in walls)
            {
                if (wall.Type.Contains("Vertical"))
                {
                    availableDirections[wall.DiscreteCoordinate.X, wall.DiscreteCoordinate.Y] &= ~Direction.Left;
                    availableDirections[wall.DiscreteCoordinate.X - 1, wall.DiscreteCoordinate.Y] &= ~Direction.Right;
                }
                else if (wall.Type.Contains("Horizontal"))
                {
                    availableDirections[wall.DiscreteCoordinate.X, wall.DiscreteCoordinate.Y] &= ~Direction.Up;
                    availableDirections[wall.DiscreteCoordinate.X, wall.DiscreteCoordinate.Y - 1] &= ~Direction.Down;
                }
            }
            return availableDirections;
        }

        public static Point AbsoluteCoordinateToDiscrete(Point absolute)
        {
            int x = (absolute.X + MapWidth / 2) / CellSize + 1;
            int y = (absolute.Y - MapHeight / 2) / CellSize * -1 + 1;
            return new Point(x, y);
        }
      
        private static Direction[,] RemoveDirectionsContactingWithBorder(int x, int y)
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
