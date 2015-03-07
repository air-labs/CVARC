using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;

namespace Demo
{
    public class DemoRobot : MoveAndGripRobot<IActorManager,MovementWorld,DemoSensorsData>
    {
	
		public override void AdditionalInitialization()
		{
			base.AdditionalInitialization();
            base.Gripper.FindDetail = () =>
				World.IdGenerator.GetAllPairsOfType<string>()
				.Where(z => World.Engine.ContainBody(z.Item2))
				.Where(z => !World.Engine.IsAttached(z.Item2))
				.Select(z => new { Item = z, Availability = Gripper.GetAvailability(z.Item2) })
				.Where(z => z.Availability.Distance < 30 && Math.Abs(z.Availability.Angle.Grad) < 30)
				.OrderBy(z => z.Availability.Distance)
				.Select(z => z.Item.Item2)
				.FirstOrDefault();
            base.Gripper.GrippingPoint = new Frame3D(15, 0, 10);
		}
	}
}
