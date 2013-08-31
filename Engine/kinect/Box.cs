using System;

namespace kinect
{
	class Box//TODO. см. Eurosim.Core.Box
	{
		//TODO.здесь будет AIRlab.Mathematics.Frame3D Location
		Point position;
		Vector xVector;
		Vector yVector;
		Vector zVector;
		//Конструктор по точке и трем направляющим
		public Box(Point p, Vector xVector, Vector yVector, Vector zVector)
		{
			position = new Point(p);
			double[] m1 = new double[3];
			m1 = xVector.GetCoord();
			this.xVector = new Vector(m1[0], m1[1], m1[2]);
			m1 = yVector.GetCoord();
			this.yVector = new Vector(m1[0], m1[1], m1[2]);
			m1 = zVector.GetCoord();
			this.zVector = new Vector(m1[0], m1[1], m1[2]);
		}
		//Конструктор по координатам "центра" и трем направляющим
		public Box(double x, double y, double z, Vector xVector, Vector yVector, Vector zVector)
		{
			position = new Point(x, y, z);
			double[] m1 = new double[3];
			m1 = xVector.GetCoord();
			this.xVector = new Vector(m1[0], m1[1], m1[2]);
			m1 = yVector.GetCoord();
			this.yVector = new Vector(m1[0], m1[1], m1[2]);
			m1 = zVector.GetCoord();
			this.zVector = new Vector(m1[0], m1[1], m1[2]);
		}
		//Установить координаты "начала"
		public void SetPosition(double x, double y, double z)
		{
			position.SetCoord(x, y, z);
		}

		//TODO. для таких простых свойств можно использовать auto-Property
		//public Point Position {get;set;}
		
		//Установить бокс в точку
		public void SetPosition(Point p)
		{
			position = new Point(p);
		}
		//Получить "начало" бокса(как точку)
		public Point GetPosition()
		{
			return position;
		}
		//Получить массив точек вершин
		public Point[] GetVertex()
		{
			Point[] vertex = new Point[8];
			vertex[0] = new Point(position);
			vertex[1] = new Point(position.PutAside(xVector));
			vertex[2] = new Point(position.PutAside(yVector));
			vertex[3] = new Point(position.PutAside(zVector));
			vertex[4] = new Point(position.PutAside(xVector.SumWith(yVector)));
			vertex[5] = new Point(position.PutAside(yVector.SumWith(zVector)));
			vertex[6] = new Point(position.PutAside(zVector.SumWith(xVector)));
			vertex[7] = new Point(position.PutAside(xVector.SumWith(yVector.SumWith(zVector))));
			return vertex;
		}
		//Порядок обработки треугольников
		public int[][] VertexOrder()
		{
			int[][] order = new int[12][];
			order[0] = new int[3] { 0, 2, 1 };
			order[1] = new int[3] { 1, 2, 4 };
			order[2] = new int[3] { 0, 5, 2 };
			order[3] = new int[3] { 0, 3, 5 };
			order[4] = new int[3] { 0, 3, 1 };
			order[5] = new int[3] { 1, 3, 6 };
			order[6] = new int[3] { 3, 5, 6 };
			order[7] = new int[3] { 6, 5, 7 };
			order[8] = new int[3] { 1, 6, 7 };
			order[9] = new int[3] { 1, 7, 4 };
			order[10] = new int[3] { 2, 5, 7 };
			order[11] = new int[3] { 2, 7, 4 };
			return order;
		}
		//Методы для получения напрявляющих
		public Vector GetX()
		{
			return xVector;
		}
		public Vector GetY()
		{
			return xVector;
		}
		public Vector GetZ()
		{
			return xVector;
		}
		//Напечатать значения полей
		public void print()
		{
			Console.Write("position: ");
			position.print();
			Console.Write("xVector: ");
			xVector.print();
			Console.Write("yVector: ");
			yVector.print();
			Console.Write("zVector: ");
			zVector.print();
		}
	}
}