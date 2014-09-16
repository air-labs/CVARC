using System.Linq;
using CVARC.Basic.Sensors;
using RepairTheStarship.Sensors;

namespace MapHelper
{
    public class Map
    {
        public StarshipObject[] Details { get; set; }
        public StarshipObject[] Walls { get; set; }
        public Direction[,] AvailableDirectionsByCoordinates { get; set; }
        public int RobotId { get; set; }
        public int OpponentRobotId { get; set; }
        public PositionData CurrentPosition { get; set; }
        public PositionData OpponentPosition { get; set; }

        public Map(PositionSensorsData data)
        {
            Init(data);
            Update(data);
        }

        private void Init(PositionSensorsData data)
        {
            RobotId = data.RobotId.Id;
            OpponentRobotId = RobotId == 0 ? 1 : 0;
        }

        public void Update(PositionSensorsData data)
        {
            CurrentPosition = data.Position.PositionsData[RobotId];
            OpponentPosition = data.Position.PositionsData[OpponentRobotId];
            Details = data.MapSensor.MapItems.Where(x => x.Tag.Contains("Detail")).Select(x => new StarshipObject
                {
                    DiscreteCoordinate = GetDiscretePosition((int)x.X, (int)x.Y),
                    AbsoluteCoordinate = new Point((int) x.X, (int) x.Y),
                    Type = x.Tag
                }).ToArray();
            Walls = data.MapSensor.MapItems.Where(IsWall).Select(w => new StarshipObject
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

        public Point GetDiscretePosition(PositionData positionData)
        {
            return GetDiscretePosition((int)positionData.X, (int)positionData.Y);
        }

        private static bool IsWall(MapItem mapItem)
        {
            return mapItem.Tag.Contains("Socket") || mapItem.Tag.Contains("Wall");
        }
    }
}