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
            //обычная геометрия, все значения, которые нужны - в rules
            //TODO: вернуть правильный DifWheelCommand, см. SimpleMovement/RulesExtension
            var period = 2 * Math.PI / rules.RotationSpeedLimit.Radian;
            var routes = Math.Abs(distance) / (2 * Math.PI * rules.WheelRadius);
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

        //Аналогично: повернуть на месте на угол Angle
        public static TCommand DWMRotate<TCommand>(this IDWMRules<TCommand> rules, AIRLab.Mathematics.Angle angle)
            where TCommand : IDWMCommand, new()
        {
            var botRadius = rules.DistanceBetweenWheels / 2;
            var arcLength = (Angle.Pi.Radian * botRadius * angle.Grad) / 180;
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
        //Аналогично: проехать по арке окружности радиуса R вправо/влево столько-то в градусах
    }
}
