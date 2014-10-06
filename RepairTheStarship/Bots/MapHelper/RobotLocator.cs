using System;
using System.Collections.Generic;
using System.Linq;
using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2.SimpleMovement;
using RepairTheStarship;
using RepairTheStarship.Sensors;

namespace RepairTheStarship.MapBuilder
{
    public class RobotLocator
    {
        private readonly Map map;

        private Direction currentDirection;
        private double realRobotAngle;
        private double expectedRobotAngle;
        private RTSWorld world;

        public RobotLocator(Map map, RTSWorld world)
        {
            this.world = world;
            this.map = map;
            currentDirection = map.RobotId == TwoPlayersId.Left ? Direction.Right : Direction.Left;
            realRobotAngle = currentDirection.ToAngle();
            expectedRobotAngle = realRobotAngle;
        }

        public void Update(BotsSensorsData sensorData)
        {
            map.Update(sensorData);
            realRobotAngle = map.CurrentPosition.Angle;
        }

        private IEnumerable<SimpleMovementCommand> GetCommandsByDirectionInternal(Direction direction)
        {
            currentDirection = direction;
            yield return CorrectRobotPosition();
            yield return world.RotateCommand(Angle.FromGrad(direction.ToAngle() - expectedRobotAngle));
            expectedRobotAngle = currentDirection.ToAngle();
            yield return CorrectRobotPosition();
            yield return world.MoveCommand(50);
        }

        public IEnumerable<SimpleMovementCommand> GetCommandsByDirection(Direction direction)
        {
            return GetCommandsByDirectionInternal(direction).Where(x => (int)x.LinearVelocity != 0 || Math.Abs(x.AngularVelocity.Grad) > 0.01);
        }

        private SimpleMovementCommand CorrectRobotPosition()
        {
            var angleError = expectedRobotAngle - realRobotAngle;
            return world.RotateCommand(Angle.FromGrad(angleError));
        }
    }
}
