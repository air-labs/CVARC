using System;

namespace kinect
{
	static class TriangleIntersection
	{
		//Определение плоскости для проецирования треугольника
		static int ProjectPlane(Plane pl1)
		{
			Vector N = new Vector(pl1.Geta(), pl1.Getb(), pl1.Getc()); //Вектор нормали
			if ((Math.Abs(N.GetCoord()[0]) >= Math.Abs(N.GetCoord()[1])) 
				&& (Math.Abs(N.GetCoord()[0]) >= Math.Abs(N.GetCoord()[2]))) return 0; //0 - наибольшая Nx - проецирование на yOz
			else if ((Math.Abs(N.GetCoord()[1]) >= Math.Abs(N.GetCoord()[2]))
					&& (Math.Abs(N.GetCoord()[1]) >= Math.Abs(N.GetCoord()[0]))) return 1; //1 - наибольшая Ny - проецирование на xOz
			else return 2; //2 - наибольшая Nz проецирование на xOy
		}
		//Проецирование точек на соответствующую плоскость
		static public double[] ProjectPoint(int i, Point p) //i - плоскость проектирования
		{
			switch (i) {
				case 0: return new double[2] { p.GetCoord()[1], p.GetCoord()[2] }; //0 - проецирование на yOz
				case 1: return new double[2] { p.GetCoord()[0], p.GetCoord()[2] }; //1 - проецирование на xOz
				case 2: return new double[2] { p.GetCoord()[0], p.GetCoord()[1] }; //2 - проецирование на xOy
				default: return new double[2] { 0, 0 }; //фиктивная ветвь
			}
		}
		//Лежит ли точка p внутри треугольника(НА ПЛОСКОСТИ!)
		//TODO. Не уверен, но посмотреть AIRLab.Mathematics.IsFromRegion
		static public bool IsInsideTriangle(double[] v0, double[] v1, double[] v2, double[] p) 
		{
			double f1 = (p[1] - v0[1]) * (v1[0] - v0[0]) 
						- (p[0] - v0[0]) * (v1[1] - v0[1]);
			if (f1 <= 0)
			{
				double f2 = (p[1] - v1[1]) * (v2[0] - v1[0])
							- (p[0] - v1[0]) * (v2[1] - v1[1]);
				if (f2 <= 0)
				{
					double f3 = (p[1] - v2[1]) * (v0[0] - v2[0])
								- (p[0] - v2[0]) * (v0[1] - v2[1]);
					if (f3 <= 0) return true;
					else return false;
				}

				else return false;
			}
			else return false; 
		}
		//Метод возвращает false, если r1 не пересекает треугольник v0v1v2
		//true, если пересекает
		//point содержит точку пересечения, distance содержит пройденное расстояние
		static public bool IsIntersect(Point v0, Point v1, Point v2, Ray r1, out Point point, out double distance)
		{
			//Переменные для получения пересечений с плоскостью
			Point point1;
			double distance1;
			Plane pl1 = new Plane(v0, v1, v2); //Плоскость содержащая треугольник
			Vector N = new Vector(pl1.Geta(), pl1.Getb(), pl1.Getc()); //Вектор нормали
			if (PlaneIntersection.IsIntersect(pl1, r1, out point1, out distance1)) //есть ли пересечение с содержащей плоскостью
			{
				int i = ProjectPlane(pl1); //Определяем куда проецировать треугольник
				double[] POINT = ProjectPoint(i, point1);
				double[] V0 = ProjectPoint(i, v0);
				double[] V1 = ProjectPoint(i, v1);
				double[] V2 = ProjectPoint(i, v2);
				if (IsInsideTriangle(V0, V1, V2, POINT))
				{
					point = new Point(point1);
					Vector vec = new Vector(r1.GetOrigin(), point1);
					distance = vec.Length();
					return true;
				}
				else 
				{
					//Фиктивные значения point и distance
					point = new Point(r1.GetOrigin());
					distance = r1.DistanceTo(0);
					return false;
				}
			}
			else
			{
				//Фиктивные значения point и distance
				point = new Point(r1.GetOrigin());
				distance = r1.DistanceTo(0);
				return false;
			}
		}
	}
}