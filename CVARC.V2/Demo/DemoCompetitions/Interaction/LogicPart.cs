using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace Demo
{
    public class InteractionLogicPart : LogicPart
    {
        public InteractionLogicPart()
            : base(
            new MovementWorld(),
            ()=>new SimpleMovementTwoPlayersKeyboardControllerPool())
        {
            Bots["Bot"]=()=>new MovingForwardBot();
        }

        public override Settings GetDefaultSettings()
        {
            return new Settings
            {
                TimeLimit = double.PositiveInfinity,
                OperationalTimeLimit = double.PositiveInfinity,
                Controllers = 
                {
                    new ControllerSettings { ControllerId=TwoPlayersId.Left, Name="Bot", Type= ControllerType.Bot },
                    new ControllerSettings { ControllerId=TwoPlayersId.Right, Name="Bot", Type= ControllerType.Bot }
                }
            };
        }

        public override INetworkController CreateNetworkController(CvarcTcpClient client)
        {
            return new NetworkController<SimpleMovementCommand>(client);
        }
    }
}
