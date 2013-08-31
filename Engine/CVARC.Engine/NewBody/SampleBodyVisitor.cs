using System;

namespace Eurosim.Core.VisitorExample
{
	/// Подумать.
	/// Visitor позволяет избавиться от штук вроде 
	/// if (body is Box){...}
	/// else if (body is Ball) {...}
	/// ...
	/// else {foreach in Nested
	/// }
	internal class SampleBodyVisitor : IBodyVisitor
	{
		public void Visit(Box visitable)
		{
			Console.WriteLine("Visited box");
			//Здесь создаем физическое тело/модель/ 
			//все что угодно для ящика
		}

		public void Visit(Ball visitable)
		{
			Console.WriteLine("Visited ball");
			//Здесь создаем физическое тело/модель/ 
			//все что угодно для шара
		}

		public void Visit(Cylinder visitable)
		{
			Console.WriteLine("Visited cyllinder");
			//Здесь создаем физическое тело/модель/ 
			//все что угодно для цилиндра
		}

		public void Visit(Body visitable)
		{
			Console.WriteLine("Visited body");
		}
		
	}
}