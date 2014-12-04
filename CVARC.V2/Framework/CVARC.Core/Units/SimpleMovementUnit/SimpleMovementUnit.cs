using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CVARC.V2.Units
{
	public class SimpleMovementUnit
	{
		IActor actor;

		public SimpleMovementUnit(IActor actor)
		{
			this.actor = actor;
		}

		public void Move(SimpleMovement command)
		{
			var location = actor.World.Engine.GetAbsoluteLocation(actor.ObjectId);

			var requestedSpeed = new Frame3D(command.LinearVelocity * Math.Cos(location.Yaw.Radian),
								   command.LinearVelocity * Math.Sin(location.Yaw.Radian), 0, Angle.Zero, command.AngularVelocity,
								   Angle.Zero);

			actor.World.Engine.SetSpeed(actor.ObjectId, requestedSpeed);
		}
	}
}
