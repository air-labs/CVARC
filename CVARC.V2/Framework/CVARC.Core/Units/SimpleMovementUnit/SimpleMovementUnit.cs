using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CVARC.V2.Units
{
	public class SimpleMovementUnit : IUnit<ISimpleMovementCommand>
	{
		IActor actor;

		public SimpleMovementUnit(IActor actor)
		{
			this.actor = actor;
		}


        public UnitResponse ProcessCommand(ISimpleMovementCommand command)
        {
			var location = actor.World.Engine.GetAbsoluteLocation(actor.ObjectId);

			var requestedSpeed = new Frame3D(command.SimpleMovement.LinearVelocity * Math.Cos(location.Yaw.Radian),
								   command.SimpleMovement.LinearVelocity * Math.Sin(location.Yaw.Radian), 0, Angle.Zero, command.SimpleMovement.AngularVelocity,
								   Angle.Zero);

			actor.World.Engine.SetSpeed(actor.ObjectId, requestedSpeed);
            return UnitResponse.Accepted(command.SimpleMovement.Duration);
        }
    }
}
