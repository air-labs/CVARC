using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CVARC.V2
{
	public class SimpleMovementUnit : IUnit
	{
		IActor actor;
        ISimpleMovementRules rules;

		public SimpleMovementUnit(IActor actor)
		{
			this.actor = actor;
            rules = Compatibility.Check<ISimpleMovementRules>(this, actor.Rules);
		}


        public UnitResponse ProcessCommand(object _command)
        {
            var command = Compatibility.Check<ISimpleMovementCommand>(this, _command);
            Debugger.Log(DebuggerMessageType.Workflow, "Command accepted in SMUnit");
			var location = actor.World.Engine.GetAbsoluteLocation(actor.ObjectId);

            var c = command.SimpleMovement;
            if (c == null) return UnitResponse.Denied();

            var linear = Math.Sign(c.LinearVelocity) * Math.Min(Math.Abs(c.LinearVelocity), rules.LinearVelocityLimit);
            var angular = Math.Sign(c.AngularVelocity.Radian) * Math.Min(Math.Abs(c.AngularVelocity.Radian), rules.AngularVelocityLimit.Radian);
            var angle = location.Yaw.Radian;
            if (linear != 0) angular = 0;

            var requestedSpeed = new Frame3D(
                linear * Math.Cos(angle),
                linear * Math.Sin(angle), 
                0, 
                Angle.Zero, 
                Angle.FromRad(angular),
                Angle.Zero);

			actor.World.Engine.SetSpeed(actor.ObjectId, requestedSpeed);
            return UnitResponse.Accepted(command.SimpleMovement.Duration);
        }
    }
}
