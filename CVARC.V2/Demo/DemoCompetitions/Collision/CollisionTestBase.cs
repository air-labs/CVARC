using CVARC.V2;
using CVARC.V2.SimpleMovement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public delegate void CollisionTestEntry(CvarcClient<SensorsData, SimpleMovementCommand> client, MovementWorld world, IAsserter asserter);

    public class CollisionTestBase : DelegatedCvarcTest<SensorsData, SimpleMovementCommand, MovementWorld, MovementWorldState>
    {
        public override SettingsProposal GetSettings()
        {
            return new SettingsProposal
            {
                TimeLimit = 10,
                Controllers = new List<ControllerSettings> 
                    {
                        new ControllerSettings  { ControllerId="Left", Name="This", Type= ControllerType.Client},
                        new ControllerSettings  { ControllerId="Right", Name="Detail", Type= ControllerType.Bot}
                    }
            };
        }
        bool RobotIsRectangular;
        public override MovementWorldState GetWorldState()
        {
            return new MovementWorldState() { RectangularRobot = RobotIsRectangular };
        }

        public CollisionTestBase(CollisionTestEntry entry, bool robotIsRectangular = false)
            : base((client, world, asserter) => { entry(client, world, asserter); })
        {
            RobotIsRectangular = robotIsRectangular;
        }
    }
}
