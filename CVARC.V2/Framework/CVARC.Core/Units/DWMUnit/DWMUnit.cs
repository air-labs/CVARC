using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public class DWMUnit : IUnit
	{
		DifWheelMovement movement;
		IActor actor;
		IDWMRules rules;

		public DWMUnit(IActor actor)
		{
			this.actor = actor;
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

		public void UpdateSpeed(double currentTime)
		{
			//0) обрезать скорости в соответствие с лимитом
			//1) нужно посчитать скорость робота в данный момент времени в локальных координатах (элементарная геометрия) X - вперед, Y - влево (=0), Yaw - угловая скорость
			//2) нужно перевести эту скорость в систему глобальных координат (см. SimpleMovementUnit)
			actor.World.Engine.SetSpeed(actor.ObjectId, new AIRLab.Mathematics.Frame3D());
		}
	}
}
