using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
namespace CVARC.V2
{
	public class DWMUnit : IUnit
	{
		DifWheelMovement movement;
		IDWMRobot actor;
		IDWMRules rules;

		public DWMUnit(IActor actor)
		{
			this.actor = Compatibility.Check<IDWMRobot>(this,actor);
			actor.World.Clocks.AddTrigger(new TimerTrigger(UpdateSpeed, 0.005));
			rules = Compatibility.Check<IDWMRules>(this, actor.Rules);
		}

		public UnitResponse ProcessCommand(object _command)
		{ 
			var command = Compatibility.Check<IDWMCommand>(this,_command);
			if (command.DifWheelMovement == null) return UnitResponse.Denied();
			this.movement = command.DifWheelMovement;
			return UnitResponse.Accepted(command.DifWheelMovement.Duration);
		}
        /// <summary>
        /// Update unit speed based on wheels speed
        /// </summary>
        /// <param name="currentTime"></param>
		public void UpdateSpeed(double currentTime)
		{
			if (movement == null) return;
            //TODO:
			//0) обрезать скорости в соответствие с лимитом ???
			//1) нужно посчитать скороaсть робота в данный момент времени в локальных координатах (элементарная геометрия) X - вперед, Y - влево (=0), Yaw - угловая скорость
			//2) нужно перевести эту скорость в систему глобальных координат (см. SimpleMovementUnit)
            
            double linear = 0, angular = 0;
            var angle = actor.World.Engine.GetAbsoluteLocation(actor.ObjectId).Yaw.Radian;

            //convert unit velocity form wheel velocity

            double wheelR = rules.WheelRadius;
            double leftV = movement.LeftRotatingVelocity.Radian;
            double rightV = movement.RightRotatingVelocity.Radian;
            double distBetween = rules.DistanceBetweenWheels;
            angular = wheelR * (rightV - leftV) / distBetween;
            linear = wheelR  * (leftV + rightV) / 2;

            //convert into cvarc world velocity
 
            var unitSpeed = new AIRLab.Mathematics.Frame3D(
               linear * Math.Cos(angle),
               linear * Math.Sin(angle),
               0,
               Angle.Zero,
               Angle.FromRad(angular),
               Angle.Zero);

			actor.World.Engine.SetSpeed(actor.ObjectId, unitSpeed);

			// + добавлять соответствующие записи в actor.DWMData.EncodersHistory
		}
	}
}
