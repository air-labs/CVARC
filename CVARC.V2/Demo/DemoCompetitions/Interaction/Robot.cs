using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2.SimpleMovement;
using CVARC.V2;
using CVARC.V2.Units;

namespace Demo
{
    public class InteractionRobot<TSensorsData> : 
			SimpleMovementRobot<IActorManager, MovementWorld, InteractionSensorData>, 
			IGrippableRobot
        where TSensorsData : new()
    {
		public GripperUnit Gripper
		{
			get;
			private set; 
		}

		public override void AdditionalInitialization()
		{
			base.AdditionalInitialization();
			Gripper = new GripperUnit(this);
			Gripper.FindDetail = () =>
				World.IdGenerator.GetAllPairsOfType<string>()
				.Where(z => World.Engine.ContainBody(z.Item2))
				.Where(z => !World.Engine.IsAttached(z.Item2))
				.Select(z => new { Item = z, Availability = Gripper.GetAvailability(z.Item2) })
				.Where(z => z.Availability.Distance < 30 && Math.Abs(z.Availability.Angle.Grad) < 30)
				.OrderBy(z => z.Availability.Distance)
				.Select(z => z.Item.Item2)
				.FirstOrDefault();

		}
     
        public override void ProcessCustomCommand(string commandName)
        {
			if (commandName == GripperUnit.GripCommand)
				Gripper.Grip();
			else if (commandName == GripperUnit.ReleaseCommand)
				Gripper.Release();
        }


		
	}
}
