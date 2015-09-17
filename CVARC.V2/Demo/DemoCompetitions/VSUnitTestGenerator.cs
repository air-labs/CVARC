using CVARC.V2;
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
			var helpers = new LogicPartHelper[] { new DemoLogicPartHelper(), new DWMLogicPartHelper() };
			var builder = new StringBuilder();
			
			foreach (var helper in helpers)
			{
				var logicPart = helper.Create();
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
                    var entry = "";
                    string helperType = helper.GetType().Name.Replace("LogicPartHelper", "");
                    entry = "[Test] public void " + methodName + "() { TestRunner.Run"+ helperType + "(\"" + name + "\"); }";
                    
					builder.Append(beginning + entry + ending + "\n");
				}
			}

			builder.Insert(0, "using NUnit.Framework;\n");

			File.WriteAllText("..\\..\\..\\Demo.Tests\\Tests.cs", builder.ToString());
		}
	}
}