using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Core;

namespace CVARC.Core.Physics
{
	/// <summary>
	/// Интерфейс для обертки мира конкретного движка
	/// </summary>
	public interface IWorld
	{
		/// <summary>
		/// Просчёт в физического мира, изменившегося на время dt
		/// </summary>
		void MakeIteration(double dt, Body root);

		IPhysical MakeBox(double xsize, double ysize, double zsise);
		IPhysical MakeCyllinder(double rbottom, double rtop, double height);
	}
}
