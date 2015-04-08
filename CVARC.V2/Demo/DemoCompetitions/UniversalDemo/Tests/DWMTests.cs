using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
	partial class DemoLogicPartHelper
	{
		void LoadDWMTests(LogicPart logic, DemoRules rules)
		{
			logic.Tests["DWM_Forward"] = new RoundMovementTestBase(LocationTest(10, 0, 0, rules.DWMMove(10)));
			//вперед, назад, повороты
			//езда по дугам 
			//пара-тройка составных тестов, например скругленный прямоугольник
			
		}
	}
}
