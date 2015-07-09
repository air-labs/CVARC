using System.Linq;
using CVARC.V2;
using RoboMovies;

namespace RoboMovies.MapBuilder
{
    public class InternalMap
    {
        public StarshipObject[] Details { get; set; }
        public StarshipObject[] Walls { get; set; }
        public Direction[,] AvailableDirectionsByCoordinates { get; set; }
        public string RobotId { get; set; }
        public string OpponentRobotId { get; set; }
        public LocatorItem CurrentPosition { get; set; }
        public LocatorItem OpponentPosition { get; set; }

        public InternalMap(BotsSensorsData data)
        {
            Init(data);
            Update(data);
        }

        private void Init(BotsSensorsData data)
        {
            RobotId = data.RobotId;
            OpponentRobotId = RobotId == TwoPlayersId.Left ? TwoPlayersId.Right : TwoPlayersId.Left;
        }

        public void Update(BotsSensorsData data)
        {
            CurrentPosition = data.RobotsLocations.Where(z => z.Id == RobotId).FirstOrDefault();
            OpponentPosition = data.RobotsLocations.Where(z => z.Id == OpponentRobotId).FirstOrDefault();
            Details = data.Map.Where(x => x.Tag.Contains("Detail")).Select(x => new StarshipObject
                {
                    DiscreteCoordinate = GetDiscretePosition((int)x.X, (int)x.Y),
                    AbsoluteCoordinate = new Point((int) x.X, (int) x.Y),
                    Type = x.Tag
                }).ToArray();
            Walls = data.Map.Where(IsWall).Select(w => new StarshipObject
                {
                    DiscreteCoordinate = GetDiscretePosition((int)w.X, (int)w.Y),
                    AbsoluteCoordinate = new Point((int)w.X, (int)w.Y),
                    Type = w.Tag
                }).ToArray();
        }

        public Point GetDiscretePosition(int x, int y)
        {
            return MapBuilder.AbsoluteCoordinateToDiscrete(new Point(x, y));
        }

        public Point GetDiscretePosition(LocatorItem positionData)
        {
            return GetDiscretePosition((int)positionData.X, (int)positionData.Y);
        }

        private static bool IsWall(MapItem mapItem)
        {
            return mapItem.Tag.Contains("Socket") || mapItem.Tag.Contains("Wall");
        }
    }
}