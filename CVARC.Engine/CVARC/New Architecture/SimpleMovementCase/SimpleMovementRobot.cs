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
        where TWorld : IWorld
        where TSensorsData : ISensorsData
    {
        public SimpleMovementRobot(int controllerNumber)
            : base(controllerNumber)
        { }

        public abstract void ProcessCustomCommand(string commandName, out double nextRequestTimeSpan);

        protected override void ProcessCommand(SimpleMovementCommand command, out double nextRequestTimeSpan)
        {
            if (command.Command != null)
            {
                ProcessCustomCommand(command.Command, out nextRequestTimeSpan);
                return;
            }

            if (command.WaitForExit)
            {
                nextRequestTimeSpan = double.PositiveInfinity;
                return;
            }

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
