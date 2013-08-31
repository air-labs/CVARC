using System;

namespace kinect
{
	class Sphere//TODO. см. Eurosim.Core.Ball
	{
		public Point center;
		public double radius;
		//Конструктор по координатам и радиусу
		public Sphere(double x, double y, double z, double radius)
		{
			center = new Point(x, y, z);
			this.radius = radius;
		}
		//Конструктор по точке и радиусу
		public Sphere(Point p, double radius)
		{
			center = new Point(p);
			this.radius = radius;
		}
		//Установить координаты центра
		public void SetCenter(double x, double y, double z)
		{
			center.SetCoord(x, y, z);
		}
		//Установить сферу в точку p
		public void SetCenter(Point p)
		{
			center = new Point(p);
		}
		//Получить цетр(как точкxу)
		public Point GetCenter()
		{
			return center;
		}
		//Получить радиус сферы
		public double GetRadius()
		{
			return radius;
		}
		//Напечатать значения полей
		public void print()
		{
			Console.Write("center: ");
			center.print();
			Console.WriteLine("radius: " + radius);
		}
	}
}