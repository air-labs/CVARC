using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic;

namespace CVARC.V2.SimpleMovement
{
    public abstract class SimpleMovementRobot<TActorManager,TWorld,TSensorsData>
        : Robot<TActorManager,TWorld,TSensorsData,SimpleMovementCommand>
        where TActorManager : IActorManager
        where TWorld : ISimpleMovementWorld, IWorld
        where TSensorsData : new()
    {
        public SimpleMovementRobot(string controllerId)
            : base(controllerId)
        { }

        public abstract void ProcessCustomCommand(string commandName, out double nextRequestTimeSpan);

        protected override void ProcessCommand(SimpleMovementCommand command, out double nextRequestTimeSpan)
        {
            if (command.Command != null)
            {
                Manager.SetSpeed(new Frame3D(0, 0, 0, Angle.Zero, Angle.Zero, Angle.Zero));
                ProcessCustomCommand(command.Command, out nextRequestTimeSpan);
                return;
            }

            if (command.WaitForExit)
            {
                nextRequestTimeSpan = double.PositiveInfinity;
                ProcessCustomCommand(command.Command, out nextRequestTimeSpan);
                return;
            }

            if (Math.Abs(command.LinearVelocity) > World.LinearVelocityLimit)
                command.LinearVelocity = Math.Sign(command.LinearVelocity) * World.LinearVelocityLimit;
            if (Math.Abs(command.AngularVelocity.Grad) > World.AngularVelocityLimit.Grad)
                command.AngularVelocity = Angle.FromGrad(Math.Sign(command.AngularVelocity.Grad) * World.AngularVelocityLimit.Grad);



            var location = Manager.GetAbsoluteLocation();

            if (command.LinearVelocity != 0) command.AngularVelocity = Angle.Zero;
            var requestedSpeed = new Frame3D(command.LinearVelocity * Math.Cos(location.Yaw.Radian),
                                   command.LinearVelocity * Math.Sin(location.Yaw.Radian), 0, Angle.Zero, command.AngularVelocity,
                                   Angle.Zero);

            Manager.SetSpeed(requestedSpeed);
            nextRequestTimeSpan = command.Duration;
        }
    }
}
