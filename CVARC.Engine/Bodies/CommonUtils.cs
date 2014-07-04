using System;
using System.Drawing;
using System.Linq.Expressions;
using NUnit.Framework;

namespace CVARC.Core
{
	public static class CommonUtils
	{
		/// <summary>
		/// Правильно работающий Equals для Color.
		/// Возвращает true если равны значения цветов для каждого из каналов
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static bool RgbEquals(this Color c1, Color c2)
		{
			return c1.A == c2.A && c2.B == c1.B && c2.R == c1.R && c1.G == c2.G;
		}

		public static string GetPropertyName<TOut>(Expression<Func<TOut>> propertyLambda)
		{
			return GetPropertyNameInternal(propertyLambda);
		}

		public static string GetPropertyName<TIn, TOut>(Expression<Func<TIn, TOut>> propertyLambda)
		{
			return GetPropertyNameInternal(propertyLambda);
		}

		private static string GetPropertyNameInternal<T>(Expression<T> propertyLambda)
		{
			var me = propertyLambda.Body as MemberExpression;
			if(me == null)
				throw new ArgumentException("Invalid lambda format. Lambda must be a member access expression.");
			return me.Member.Name;
		}
		public class UtilTests
		{
			[Test]
			public void PropertyNameTests()
			{
				var result = GetPropertyName<Body, Color>(b => b.DefaultColor);
				Assert.AreEqual("DefaultColor", result);
				result = GetPropertyName(() => Environment.CurrentDirectory);
				Assert.AreEqual("CurrentDirectory", result);
				result = GetPropertyName(() => System.Environment.CurrentDirectory);
				Assert.AreEqual("CurrentDirectory", result);
				Assert.Throws<ArgumentException>(()=>GetPropertyName<Body, string>(b => Body.DefaultColorPropertyName));
				Assert.Throws<ArgumentException>(()=> GetPropertyName<Body,string>(b=>"aaa"));
			}
		}
	}
}