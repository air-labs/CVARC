using System;

namespace kinect
{
	static class SphereIntersection
	{
		//Метод возвращает false, если r1 не пересекает s1
		//true, если пересекает
		//point содержит точку пересечения, distance содержит пройденное расстояние
		static public bool IsIntersect(Sphere s1, Ray r1, out Point point, out double distance)
		{
			//переменные для возврата корней квадратного уравнения
			double root1, root2;
			//Идет расчет коэффициентов решения системы уравнений сферы и луча
			double a = r1.GetDirection().SProduct(r1.GetDirection());
			//Вспомогательный вектор
			Vector vec = new Vector(s1.GetCenter(), r1.GetOrigin());
			double b = 2 * r1.GetDirection().SProduct(vec);
			double c = vec.SProduct(vec) - Math.Pow(s1.GetRadius(), 2);
			//Обработка решения
			if ((QuadEquation.Solution(a, b, c, out root1, out root2)) && ((root1 >= 0) | (root2 >= 0)))
				if (root1 >= 0)
				{
					point = r1.CalculatePoint(root1);
					distance = r1.DistanceTo(root1);
					return true;
				}
				else
				{
					point = r1.CalculatePoint(root2);
					distance = r1.DistanceTo(root2);
					return true;
				}
			else
			{
				//TODO. Не нужно придумывать значения point и distance. Это лишний шум, их все равно никто не увидит
				//point = new Point(r1.GetOrigin());
				//distance = r1.DistanceTo(0);
				point = default(Point);
				distance = default(double);
				return false;
			}
		}
	}
}