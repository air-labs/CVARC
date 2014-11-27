using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.Basic;

namespace CVARC.V2.SimpleMovement
{
    public abstract class SimpleMovementRobot<TActorManager,TWorld,TSensorsData>
        : Robot<TActorManager,TWorld,TSensorsData,SimpleMovementCommand>, ISimpleMovementRobot
        where TActorManager : IActorManager
        where TWorld : ISimpleMovementWorld, IWorld
        where TSensorsData : new()
    {

        public virtual void ProcessCustomCommand(string commandName)
        { }



        public override void ExecuteCommand(SimpleMovementCommand command)
        {
            if (command.Command != null)
            {
                Manager.SetSpeed(new Frame3D(0, 0, 0, Angle.Zero, Angle.Zero, Angle.Zero));
                ProcessCustomCommand(command.Command);
                return;
            }

            var location = Manager.GetAbsoluteLocation();

            var requestedSpeed = new Frame3D(command.LinearVelocity * Math.Cos(location.Yaw.Radian),
                                   command.LinearVelocity * Math.Sin(location.Yaw.Radian), 0, Angle.Zero, command.AngularVelocity,
                                   Angle.Zero);

            Manager.SetSpeed(requestedSpeed);
        }


        ISimpleMovementWorld ISimpleMovementRobot.World
        {
            get { return World; }
        }
    }
}
