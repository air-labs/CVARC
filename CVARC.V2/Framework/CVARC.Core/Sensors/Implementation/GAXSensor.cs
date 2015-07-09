using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;

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
        Queue<Frame3D> positions = new Queue<Frame3D>();

		public void Register(double time)
		{
            var currentLocation = Actor.World.Engine.GetAbsoluteLocation(Actor.ObjectId);
            positions.Enqueue(currentLocation);
            if (positions.Count == 3)
            {
                var timestamp = Actor.World.Clocks.CurrentTime - GAXDelta;

                //calculate acceleration vector

                var pos = positions.ToArray();
                var ax = (pos[2].X - 2 * pos[1].X + pos[0].X) / Math.Pow(GAXDelta, 2.0);
                var ay = (pos[2].Y - 2 * pos[1].Y + pos[0].X) / Math.Pow(GAXDelta, 2.0);

                var velocityAroundZ = (pos[2].Yaw - 2 * pos[1].Yaw + pos[0].Yaw) / Math.Pow(GAXDelta, 2.0);

                buffer.Add(new GAXData
                {
                    TimeStamp = timestamp,
                    Accelerations = new Point3D(ax, ay, 0.0),
                    VelocityAroundX = Angle.Zero,
                    VelocityAroundY = Angle.Zero,
                    VelocityAroundZ = velocityAroundZ
                });
                positions.Clear();
            }
           
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
