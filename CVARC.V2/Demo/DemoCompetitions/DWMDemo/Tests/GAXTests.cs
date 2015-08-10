using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;

namespace Demo
{
	partial class DWMLogicPartHelper
	{
        DWMTestEntry GAXTest(Point3D accelerations, Angle AroundX, Angle AroundY, Angle AroundZ, params DWMCommand[] command)
        {
            return (client, world, asserter) =>
            {
				DWMSensorsData data = new DWMSensorsData();
                foreach (var c in command)
                    data = client.Act(c);
                    
                asserter.IsEqual(accelerations.X, data.GAX.Last().Accelerations.X, 0);
                asserter.IsEqual(accelerations.Y, data.GAX.Last().Accelerations.Y, 0);
                asserter.IsEqual(accelerations.Z, data.GAX.Last().Accelerations.Z, 0);
                asserter.IsEqual(AroundZ.Radian, data.GAX.Last().VelocityAroundZ.Radian, 0);
            };
        }

		void LoadGAXTests(LogicPart logic, DWMRules rules)
		{
			//1. сделать ступенчатое движение с ускорением и посчитать среднее ускорение
			//2. Пустить робота по кругу и проверить, что ускорение смотрит в центр
			//3. Аналогично с угловой скоростью
			//На DWM-е
            logic.Tests["GAX_CircleMoving"] = new DWMMovementTestBase(
                       GAXTest(new Point3D(0, -3, 0), Angle.Zero, Angle.Zero, Angle.Zero,
                       rules.DWMMoveArc(3, Angle.FromGrad(360), false),
                       rules.DWMStand(1.0)));
            logic.Tests["GAX_Rotating"] = new DWMMovementTestBase(
                       GAXTest(new Point3D(0, 0, 0), Angle.Zero, Angle.Zero, Angle.HalfPi,
                       rules.DWMRotate(Angle.Pi), rules.DWMStand(1.0)));

			
		}
	}
}
