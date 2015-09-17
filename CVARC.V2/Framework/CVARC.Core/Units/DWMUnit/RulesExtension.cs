using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;

namespace CVARC.V2
{
    public static class RulesExtension
    {
        public static TCommand DWMStand<TCommand>(this IDWMRules<TCommand> rules, double duration)
            where TCommand : IDWMCommand, new()
        {
            return new TCommand
            {
                DifWheelMovement = new DifWheelMovement
                {
                    Duration = duration,
                    LeftRotatingVelocity = AIRLab.Mathematics.Angle.Zero,
                    RightRotatingVelocity = AIRLab.Mathematics.Angle.Zero

                }
            };
        }
        public static TCommand DWMMoveForward<TCommand>(this IDWMRules<TCommand> rules, double distance)
            where TCommand : IDWMCommand, new()
        {
            var period = 2.0 * Math.PI / rules.RotationSpeedLimit.Radian;
            var routes = Math.Abs(distance) / (2.0 * Math.PI * rules.WheelRadius);
            var duration = routes * period;
            var wheelSpeed = rules.RotationSpeedLimit;
            return new TCommand
            {
                DifWheelMovement = new DifWheelMovement
                    {
                        Duration = duration,
                        LeftRotatingVelocity = Math.Sign(distance) * wheelSpeed,
                        RightRotatingVelocity = Math.Sign(distance) * wheelSpeed
                    }
            };
        }

        /// <summary>
        ///robot pivots around itself 
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="rules"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static TCommand DWMRotate<TCommand>(this IDWMRules<TCommand> rules, AIRLab.Mathematics.Angle angle)
            where TCommand : IDWMCommand, new()
        {
            var botRadius = rules.DistanceBetweenWheels / 2;
            var arcLength = botRadius * angle.Radian;
            var routes = Math.Abs(arcLength) / (2 * Math.PI * rules.WheelRadius);
            var period = 2 * Math.PI / rules.RotationSpeedLimit.Radian;
            var duration = routes * period;
            return new TCommand
            {
                DifWheelMovement = new DifWheelMovement
                {
                    Duration = duration,
                    LeftRotatingVelocity = Math.Sign(angle.Radian) * rules.RotationSpeedLimit,
                    RightRotatingVelocity = - Math.Sign(angle.Radian) * rules.RotationSpeedLimit
                }
            };
        }
        /// <summary>
        /// robot moves along an arc
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="rules"></param>
        /// <param name="arcRadius"></param>
        /// <param name="angle"></param>
        /// <param name="direction">true for moving right, and false to moving left</param>
        /// <returns></returns>
        public static TCommand DWMMoveArc<TCommand>(this IDWMRules<TCommand> rules,
                                                         double arcRadius,
                                                         AIRLab.Mathematics.Angle angle,
                                                         bool direction)
            where TCommand : IDWMCommand, new()
        {
            double innerArcLength = arcRadius * angle.Radian;
            double outerArcLength = (arcRadius + rules.DistanceBetweenWheels) * angle.Radian;
            var arcDiff = outerArcLength / innerArcLength;

            var routes = Math.Abs(outerArcLength) / (2 * Math.PI * rules.WheelRadius);
            var period = 2 * Math.PI / rules.RotationSpeedLimit.Radian;
            var duration = routes * period;
          
            var rightRotatingVelocity = Angle.Zero;
            var leftRotatingVelocity = Angle.Zero;

            if (direction)
            {
                leftRotatingVelocity = rules.RotationSpeedLimit;
                rightRotatingVelocity = Angle.FromRad(leftRotatingVelocity.Radian / arcDiff);
            }
            else
            {
                rightRotatingVelocity = rules.RotationSpeedLimit;
                leftRotatingVelocity = Angle.FromRad(rightRotatingVelocity.Radian / arcDiff);
            }
            return new TCommand
            {
                DifWheelMovement = new DifWheelMovement
                {
                    Duration = duration,
                    LeftRotatingVelocity = leftRotatingVelocity,
                    RightRotatingVelocity = rightRotatingVelocity
                }
            };
        }
        //Аналогично: проехать по арке окружности радиуса R вправо/влево столько-то в градусах
    }
}
