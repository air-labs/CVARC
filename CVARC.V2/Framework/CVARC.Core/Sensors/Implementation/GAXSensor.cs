using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public class GAXSensor : Sensor<List<GAXData>,IActor>
	{

		const double GAXDelta = 0.005;

		public override void Initialize(IActor actor)
		{
			base.Initialize(actor);
			actor.World.Clocks.AddTrigger(new TimerTrigger(Register, GAXDelta));
			
		}

		List<GAXData> buffer = new List<GAXData>();

		public void Register(double time)
		{
			var currentLocation = Actor.World.Engine.GetAbsoluteLocation(Actor.ObjectId);
			//посчитать ускорения по этому и предыдущим положения (предыдущие положения сохранять в очереди из трех положений)
			//сложить в буфер
		}

		public override List<GAXData> Measure()
		{
			return buffer;
			buffer = new List<GAXData>();
		}
	}
}
