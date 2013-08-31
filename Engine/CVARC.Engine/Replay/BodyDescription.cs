using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eurosim.Core.Replay
{
	[Serializable]
	public abstract class BodyDescription
	{
		public BodyDescription() { }

		//public abstract void UpdateBody(PrimitiveBody pb);

		//public abstract bool HasNextChange();

		//public abstract void SaveBody(PrimitiveBody body);

		public double StartTime;

		/// <summary>
		/// Является ли это действие единичным, т.е. без последующих итераций
		/// </summary>
		public bool IsSingleAct = false;
	}
}
