using RepairTheStarship;
using RepairTheStarship.Sensors;

namespace Gems.Robots
{
    public class ImageGemsRobot : GemsRobot
    {
        public ImageGemsRobot(SRCompetitions competitions, int number)
            : base(competitions, number)
        {
        }

        public override T GetSensorsData<T>()
        {
            return new ImageSensorsData
            {
                RobotId = RobotIdSensor.Measure(),
                Position = PositionSensor.Measure(),
                Image = RobotCamera.Measure(),
                DetailsInfo = GripSensor.Measure()
            } as T;
        }
    }
}