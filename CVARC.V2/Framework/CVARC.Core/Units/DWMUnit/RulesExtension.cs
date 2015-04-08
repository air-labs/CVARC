using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public static class RulesExtension
	{
		public static TCommand DWMMove<TCommand>(this IDWMRules<TCommand> rules, double distance)
			where TCommand : IDWMCommand
		{
			//обычная геометрия, все значения, которые нужны - в rules
			//TODO: вернуть правильный DifWheelCommand, см. SimpleMovement/RulesExtension
			return default(TCommand);
		}

		//Аналогично: повернуть на месте на угол Angle

		//Аналогично: проехать по арке окружности радиуса R вправо/влево столько-то в градусах
	}
}
