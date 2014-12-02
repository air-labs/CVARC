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

        int collisionTime = 0;
        bool firstTime = true;

        public override SimpleMovementCommand GetCommand()
        {
            if (take && firstTime)
            {
                firstTime = false;
                return SimpleMovementCommand.Action("Grip");
            }
            return SimpleMovementCommand.Stand(1);
            //var thatLocation = actor.World.Engine.GetAbsoluteLocation(actor.World.Actors.Where(z => z.ControllerId != actor.ControllerId).FirstOrDefault().ObjectId);
            //var selfLocation = actor.World.Engine.GetAbsoluteLocation(actor.ObjectId);
            //var relative = selfLocation.Invert().Apply(thatLocation);
            //
            //var targetAngle = Angle.FromRad(Math.Atan2(relative.Y, relative.X));
            //if (!forward) targetAngle += Angle.Pi;
            //targetAngle=targetAngle.Simplify180();
            //if (Math.Abs(targetAngle.Grad) > 1)
            //    return SimpleMovementCommand.RotateWithVelocity(targetAngle, Angle.FromGrad(90));
            //
            //
            //if (Math.Abs(relative.X) < 22)
            //    collisionTime++;
            //if (collisionTime > 10)
            //{
            //    collisionTime = 0;
            //    return SimpleMovementCommand.Move(forward ? -50 : 50, 0.5);
            //}
            //    
            //return SimpleMovementCommand.Move(forward ? 50 : -50, 0.1);
        }
    }
}
