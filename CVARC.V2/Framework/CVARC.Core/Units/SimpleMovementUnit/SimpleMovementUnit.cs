using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CVARC.V2.Units
{
	public class SimpleMovementUnit : IUnit
	{
		IActor actor;

		public SimpleMovementUnit(IActor actor)
		{
			this.actor = actor;
		}


        public UnitResponse ProcessCommand(object _command)
        {
            var command=Compatibility.Check<ISimpleMovementCommand>(this,_command).SimpleMovement;
			var location = actor.World.Engine.GetAbsoluteLocation(actor.ObjectId);

			var requestedSpeed = new Frame3D(command.LinearVelocity * Math.Cos(location.Yaw.Radian),
								   command.LinearVelocity * Math.Sin(location.Yaw.Radian), 0, Angle.Zero, command.AngularVelocity,
								   Angle.Zero);

			actor.World.Engine.SetSpeed(actor.ObjectId, requestedSpeed);
            return UnitResponse.Accepted(command.Duration);
        }
    }
}
