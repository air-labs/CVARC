using System;

namespace CVARC.Graphics.DirectX.Utils
{
	public static class DynamicConverterExtensions
	{
		public static TOut Convert<TIn, TOut>(this TIn input, IConverter<TIn, TOut> converter)
		{
			if(input.Equals(null))
				throw new ArgumentNullException();
			TOut result;
			try
			{
				result = ((dynamic)converter).Convert((dynamic)input);
			}
			catch
			{
				converter.OnError(input);
				throw;
			}
			return result;
		}
	}
}

namespace CVARC.Graphics.DirectX
{
	public interface IConverter<in TIn, out TOut>
	{
		TOut Convert(TIn t);
		void OnError(TIn t);
	}
}