using System;
using System.Collections.Generic;
using System.Linq;
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

        public RobotLocator(Map map)
        {
            this.map = map;
            currentDirection = map.RobotId == 0 ? Direction.Right : Direction.Left;
            realRobotAngle = currentDirection.ToAngle();
            expectedRobotAngle = realRobotAngle;
        }

        public void Update(PositionSensorsData sensorData)
        {
            map.Update(sensorData);
            realRobotAngle = map.CurrentPosition.Angle;
        }

        private IEnumerable<Command> GetCommandsByDirectionInternal(Direction direction)
        {
            currentDirection = direction;
            yield return CorrectRobotPosition();
            yield return Command.Rot(direction.ToAngle() - expectedRobotAngle);
            expectedRobotAngle = currentDirection.ToAngle();
            yield return CorrectRobotPosition();
            yield return Command.Mov(50);
        }

        public IEnumerable<Command> GetCommandsByDirection(Direction direction)
        {
            return GetCommandsByDirectionInternal(direction).Where(x => (int)x.Move != 0 || Math.Abs(x.Angle.Grad) > 0.01);
        }

        private Command CorrectRobotPosition()
        {
            var angleError = expectedRobotAngle - realRobotAngle;
            return Command.Rot(angleError);
        }
    }
}
