using AIRLab.Mathematics;
using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public delegate void DWMTestEntry(CvarcClient<DWMSensorsData, DWMCommand> client, DWMWorld world, IAsserter asserter);
    public class DWMMovementTestBase : DWMTestBase
    {
        public DWMMovementTestBase(DWMTestEntry entry)
            : base(entry, new DWMWorldState(0, 0)) { }
    }

    public class DWMDistortionTestBase : DWMTestBase
    {
        public DWMDistortionTestBase(DWMTestEntry entry)
            : base(entry, new DWMWorldState(10, 1)) { }
    }

    public class DWMTestBase : DelegatedCvarcTest<DWMSensorsData, DWMCommand, DWMWorld, DWMWorldState>
    {
        public override SettingsProposal GetSettings()
        {
            return new SettingsProposal
            {
                TimeLimit = 10,
                Controllers = new List<ControllerSettings> 
                    {
                        new ControllerSettings  { ControllerId=TwoPlayersId.Left, Name="This", Type= ControllerType.Client},
                    }
            };
        }

        DWMWorldState WorldState;

        public override DWMWorldState GetWorldState()
        {
            return WorldState;
        }

        public DWMTestBase(DWMTestEntry entry, DWMWorldState state)
            : base((client, world, asserter) => { entry(client, world, asserter); })
        {
            WorldState = state;
        }
    }
}
