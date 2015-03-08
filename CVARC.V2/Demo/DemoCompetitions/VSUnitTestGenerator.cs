using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Demo
{
	class VSUnitTestGenerator
	{
		public static void Main()
		{
			var helper = new DemoLogicPartHelper();
			var logicPart = helper.Create();
			var builder = new StringBuilder();
			foreach (var e in logicPart.Tests)
			{
				var name = e.Key;
				var nameParts = name.Split('_');

				string beginning = "";
				string ending = "";

				foreach (var className in nameParts.Take(nameParts.Length - 2))
				{
					beginning += "namespace " + className + " { \n";
					ending += "}";
				}

				beginning += "[TestFixture] public partial class " + nameParts[nameParts.Length - 2] + " {\n";
				ending += "}";

				var methodName = nameParts[nameParts.Length - 1];
				var entry = "[Test] public void " + methodName + "() { TestRunner.Run(\"" + name + "\"); }";

				builder.Append(beginning + entry + ending + "\n");
			}

			builder.Insert(0, "using NUnit.Framework;\n");

			File.WriteAllText("..\\..\\..\\Demo.Tests\\Tests.cs", builder.ToString());
		}
	}
}