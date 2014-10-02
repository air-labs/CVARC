namespace CVARC.Core
{
	public interface IVisitable<in T>
	{
		void AcceptVisitor(T visitor);
	}

	public interface IBodyVisitor
	{
		void Visit(Box visitable);
		void Visit(Ball visitable);
		void Visit(Cylinder visitable);
		void Visit(Body visitable);
	}
}
