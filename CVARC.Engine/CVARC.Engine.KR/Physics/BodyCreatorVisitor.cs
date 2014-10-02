using System.Collections.Generic;
using CVARC.Core;
using CVARC.Core.Physics;
using CVARC.Physics.Exceptions;

namespace CVARC.Physics
{
	/// <summary>
	/// Посетитель тел, нужный для создания соответствующих физических тел
	/// </summary>
	internal class BodyCreatorVisitor : IBodyVisitor
	{
		public void Visit(Box visitable)
		{
			IPhysical physical = PhysicalManager.MakeBox(visitable.XSize, visitable.YSize, visitable.ZSize);
		    physical.Body = visitable;
			AfterCreating(visitable, physical);
		}

		public void Visit(Ball visitable)
		{
			IPhysical physical = PhysicalManager.MakeCyllinder(visitable.Radius, visitable.Radius, visitable.Radius * 2);
            physical.Body = visitable;
            AfterCreating(visitable, physical);
		}

		public void Visit(Cylinder visitable)
		{
			IPhysical physical = PhysicalManager.MakeCyllinder(visitable.RBottom, visitable.RTop, visitable.Height);
            physical.Body = visitable;
            AfterCreating(visitable, physical);
		}

		public void Visit(Body visitable)
		{
			throw new UnknownBodyTypeException("BodyCreatorVisitor got visitable body without type of shape");
		}

		private void AfterCreating(Body body, IPhysical physical)
		{
			PhysicalManager.SetSettings(body, physical);
			PhysicalManager.SaveBody(body, physical);
		}
	}
}
