using CVARC.Basic.Sensors;
using RepairTheStarship.Sensors;

namespace MapHelper
{
    public class Map
    {
        public Wall[] Walls { get; set; }
        public Direction[,] AvailableDirectionsByCoordinates { get; set; }
        public int RobotId { get; set; }
        public int OpponentRobotId { get; set; }
        public PositionData CurrentPosition { get; set; }
        public PositionData OpponentPosition { get; set; }

        public Map(SensorsData data)
        {
            Init(data);
            Update(data);
        }

        private void Init(SensorsData data)
        {
            RobotId = data.RobotIdSensor.Id;
            OpponentRobotId = RobotId == 0 ? 1 : 0;
        }

        public void Update(SensorsData data)
        {
            CurrentPosition = data.LightHouseSensor.PositionsData[RobotId];
            OpponentPosition = data.LightHouseSensor.PositionsData[OpponentRobotId];
        }
    }
}