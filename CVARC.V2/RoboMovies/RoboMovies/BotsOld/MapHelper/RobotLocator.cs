using System;
using System.Collections.Generic;
using System.Linq;
using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2;
using RoboMovies;
using RoboMovies.Sensors;

namespace RoboMovies.MapBuilder
{
    public class RobotLocator
    {
        private readonly InternalMap map;

        private Direction currentDirection;
        private double realRobotAngle;
        private double expectedRobotAngle;
        private RMWorld world;

        public RobotLocator(InternalMap map, RMWorld world)
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

        private IEnumerable<MoveAndBuildCommand> GetCommandsByDirectionInternal(Direction direction)
        {
            currentDirection = direction;
            yield return CorrectRobotPosition();
            var directionAngle = direction.ToAngle();
            yield return RMRules.Current.Rotate(Normilize(Angle.FromGrad(directionAngle - expectedRobotAngle)));
            expectedRobotAngle = currentDirection.ToAngle();
            yield return CorrectRobotPosition();
            yield return RMRules.Current.Move(50);
        }

        public IEnumerable<MoveAndBuildCommand> GetCommandsByDirection(Direction direction)
        {
            return GetCommandsByDirectionInternal(direction).Where(x => (int)x.SimpleMovement.LinearVelocity != 0 || Math.Abs(x.SimpleMovement.AngularVelocity.Grad) > 0.01);
        }

        private MoveAndBuildCommand CorrectRobotPosition()
        {
            var angleError = expectedRobotAngle - realRobotAngle;
            return RMRules.Current.Rotate(Normilize(Angle.FromGrad(angleError)));
        }

        public Angle Normilize(Angle angle)
        {
            var grad = angle.Grad % 360;
            if (grad > 180)
                grad = grad - 360;
            if (grad < -180)
                grad = 360 + grad;
            return Angle.FromGrad(grad);
        }
    }
}
