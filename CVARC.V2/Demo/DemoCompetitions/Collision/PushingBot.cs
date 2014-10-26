using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace Demo
{
    public class PushingBot : Controller<SimpleMovementCommand>
    {
        bool forward;
        bool take;
        int stage;

        public PushingBot(bool take, bool forward)
        {
            this.forward = forward;
            this.take = take;
        }
        IActor actor;

        public override void Initialize(IActor controllableActor)
        {
            this.actor = controllableActor;
        }

        public override SimpleMovementCommand GetCommand()
        {
            if (take && stage == 0) return SimpleMovementCommand.Action("Grip");
            var thatLocation = actor.World.Engine.GetAbsoluteLocation(actor.World.Actors.Where(z => z.ControllerId != actor.ControllerId).FirstOrDefault().ObjectId);
            var selfLocation = actor.World.Engine.GetAbsoluteLocation(actor.ObjectId);
            var relative = selfLocation.Invert().Apply(thatLocation);

            var targetAngle = Angle.FromRad(Math.Atan2(relative.Y, relative.X));
            if (!forward) targetAngle += Angle.Pi;
            targetAngle=targetAngle.Simplify180();
            if (Math.Abs(targetAngle.Grad) > 1)
                return SimpleMovementCommand.RotateWithVelocity(targetAngle, Angle.FromGrad(50));
            return SimpleMovementCommand.Move(forward ? 50 : -50, 1);
        }
    }
}
