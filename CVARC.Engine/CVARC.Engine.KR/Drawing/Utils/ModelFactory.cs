using CVARC.Core;

namespace CVARC.Graphics
{
	public abstract class ModelFactory<TOut> : IBodyVisitor
	{
		public abstract void Visit(Box visitable);

		public abstract void Visit(Ball visitable);

		public abstract void Visit(Cylinder visitable);

		public abstract void Visit(Body visitable);

		public bool TryGetResult(Body input, out TOut result)
		{
			InternalResult = default(TOut);
			input.AcceptVisitor(this);
			result = InternalResult;
			return !Equals(result, default(TOut));
		}

		protected TOut InternalResult { get; set; }
	}
}