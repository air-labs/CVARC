using System;

namespace kinect
{
	class Plane//TODO. см. AIRLab.Mathematics.Plane
	{
		//Плоскость ax+by+cz+d=0
		double a, b, c, d;
		//Конструктор по коэффициентам уравнения
		public Plane(double a, double b, double c, double d)
		{
			this.a = a;
			this.b = b;
			this.c = c;
			this.d = d;
		}
		//Конструктор по трем точкам
		public Plane(Point k, Point m, Point l)
		{
			Vector vec1 = new Vector(k, m);
			Vector vec2 = new Vector(k, l);
			Vector vec = vec1.Vproduct(vec2);
			a = vec.GetCoord()[0];
			b = vec.GetCoord()[1];
			c = vec.GetCoord()[2];
			d = -a * k.GetCoord()[0] - b * k.GetCoord()[1] - c * k.GetCoord()[2];
		}
		//Проверить принадлежность точки плоскости
		public bool Belong(Point p)
		{
			double k = a * p.GetCoord()[0] + b * p.GetCoord()[1] + c * p.GetCoord()[2] + d;
			if (k == 0) return true;
			else return false;
		}
		//Напечатать уравнение плоскости
		public void print()
		{
			Console.WriteLine(a + "x + " + b + "y + " + c + "z + " + d +  " = 0");
		}
		//Группа методов для получения коэффициентов
		public double Geta()
		{
			return a;
		}
		public double Getb()
		{
			return b;
		}
		public double Getc()
		{
			return c;
		}
		public double Getd()
		{
			return d;
		}
	}
}