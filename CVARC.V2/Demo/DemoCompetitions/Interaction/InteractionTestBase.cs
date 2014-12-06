using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public delegate void InteractionTestEntry(CvarcClient<InteractionSensorData, MoveAndGripCommand> client, MovementWorld world, IAsserter asserter);

    public class InteractionTestBase : DelegatedCvarcTest<InteractionSensorData, MoveAndGripCommand, MovementWorld, MovementWorldState>
    {
        public override SettingsProposal GetSettings()
        {
            return new SettingsProposal
            {
                TimeLimit = 10,
                Controllers = new List<ControllerSettings> 
                    {
                        new ControllerSettings  { ControllerId="Left", Name="This", Type= ControllerType.Client}
                    }
            };
        }
        bool RobotIsRectangular;
        public override MovementWorldState GetWorldState()
        {
            return new MovementWorldState() { RectangularRobot = RobotIsRectangular };
        }

        public InteractionTestBase(InteractionTestEntry entry, bool robotIsRectangular = false)
            : base((client, world, asserter) => { entry(client, world, asserter); })
        {
            RobotIsRectangular = robotIsRectangular;
        }
    }
}
