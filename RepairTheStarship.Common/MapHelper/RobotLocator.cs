using System.Collections.Generic;
using CVARC.Basic.Controllers;
using RepairTheStarship.Sensors;

namespace MapHelper
{
    public class RobotLocator
    {
        private readonly Map map;

        private Direction currentDirection;
        private double realRobotAngle;
        private double expectedRobotAngle;
        private int angleSign;

        public RobotLocator(Map map)
        {
            this.map = map;
            currentDirection = map.RobotId == 0 ? Direction.Right : Direction.Left;
            angleSign = map.RobotId == 0 ? 1 : -1;
            realRobotAngle = currentDirection.ToAngle();
            expectedRobotAngle = realRobotAngle;
        }

        public void Update(SensorsData sensorData)
        {
            map.Update(sensorData);
            realRobotAngle = map.CurrentPosition.Angle;
        }

        public IEnumerable<Command> GetCommandsByDirection(Direction direction)
        {
            currentDirection = direction;
            yield return CorrectRobotPosition();
            yield return Command.Rot(direction.ToAngle() - expectedRobotAngle);
            expectedRobotAngle = currentDirection.ToAngle();
            yield return CorrectRobotPosition();
            yield return Command.Mov(50);
        }

        private Command CorrectRobotPosition()
        {
            var angleError = expectedRobotAngle - realRobotAngle * angleSign;
            return Command.Rot(angleError);
        }
    }
}
