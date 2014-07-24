using RepairTheStarship.Sensors;

namespace MapHelper
{
    public class Map
    {
        public Wall[] Walls { get; set; }
        public Direction[,] AvailableDirectionsByCoordinates { get; set; }
        public int CurrentRobotId { get;private set; }
        public double CurrentRobotAngle { get; set; }

        public Map(SensorsData data)
        {
            CurrentRobotId = data.RobotIdSensor.Id;
            SetAngle(data);
        }

        public void Update(SensorsData data)
        {
            SetAngle(data);
        }

        private void SetAngle(SensorsData data)
        {
            CurrentRobotAngle = data.LightHouseSensor.PositionsData[CurrentRobotId].Angle;
        }
    }
}