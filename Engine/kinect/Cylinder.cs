using System;

namespace kinect
{
	class Cylinder
	{
		Point center;
		double radius;
		double height;
		//Конструктор по точке и размерам
		public Cylinder(Point p, double radius, double height)
		{
			center = new Point(p);
			this.radius = radius;
			this.height = height;
		}
		//Конструктор по координатам центра и размерам
		public Cylinder(double x, double y, double z, double radius, double height)
		{
			center = new Point(x, y, z);
			this.radius = radius;
			this.height = height;
		}
		//Установить координаты центра
		public void SetCenter(double x, double y, double z)
		{
			center.SetCoord(x, y, z);
		}
		//Установить цилиндр в точку
		public void SetCenter(Point p)
		{
			center = new Point(p);
		}
		//Получить центр(как точку)
		public Point GetCenter()
		{
			return center;
		}
		//Получить радиус
		public double GetRadius()
		{
			return radius;
		}
		//Получить высоту
		public double GetHeight()
		{
			return height;
		}
		//Напечатать значения полей
		public void print()
		{
			Console.Write("center: ");
			center.print();
			Console.WriteLine("radius: " + radius);
			Console.WriteLine("height: " + height);
		}
	}
}