using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace RoboMovies
{
    public delegate void RMTestEntry(CvarcClient<FullMapSensorData, RMCommand> client, RMWorld world, IAsserter asserter);

    public class RMTestBase : DelegatedCvarcTest<FullMapSensorData, RMCommand, RMWorld, RMWorldState>
    {
        public override SettingsProposal GetSettings()
        {
            return new SettingsProposal
            {
                TimeLimit = 30,
                Controllers = new List<ControllerSettings> 
                    {
                        new ControllerSettings  { ControllerId=TwoPlayersId.Left, Name="This", Type= ControllerType.Client},
                        new ControllerSettings  { ControllerId=TwoPlayersId.Right, Name="Stand", Type= ControllerType.Bot}
                    }
            };
        }

        RMWorldState worldState;

        public override RMWorldState GetWorldState()
        {
            return worldState;
        }

        public RMTestBase(RMTestEntry entry, RMWorldState state)
            : base((client, world, asserter) => { entry(client, world, asserter); })
        {
            worldState = state;
        }
    }
}
