using CVARC.V2;
using CVARC.V2.SimpleMovement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public class MovementTestBase : DelegatedCvarcTest<SensorsData,SimpleMovementCommand,MovementWorld>
    {
        public override ConfigurationProposal GetConfiguration()
        {
            return new ConfigurationProposal
            {
                LoadingData = new LoadingData
                {
                    AssemblyName = "Demo",
                    Level = "Movement"
                },
                SettingsProposal = new SettingsProposal
                {
                    TimeLimit = 10,
                    Controllers = new List<ControllerSettings> 
                    {
                        new ControllerSettings  { ControllerId=MovementLogicPart.ControllerId, Name="This", Type= ControllerType.Client}
                    }
                }
            };
        }

        public MovementTestBase(Action<CvarcClient<SensorsData, SimpleMovementCommand>, MovementWorld, IAsserter> test)
            : base(test)
        { }
    }
}
