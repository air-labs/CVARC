using AIRLab.Mathematics;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jango
{
    public class JangoRobot : Robot<JangoActorManager, JangoWorld, object, JangoCommand>
    {
        public JangoRobot(string controllerId)
            : base(controllerId)
        { }

        public override void ExecuteCommand(JangoCommand command)
        {
            var baseJointId = World.IdGenerator
                .GetIdFor(Parts.BaseJoint).First();
            World.Engine.SetSpeed(baseJointId,
                new Frame3D(0, 0, 0, 
                    command.RequestedAngleDeltas[0] / command.Duration,
                    command.RequestedAngleDeltas[1] / command.Duration, 
                    Angle.Zero));
        }
    }
}
