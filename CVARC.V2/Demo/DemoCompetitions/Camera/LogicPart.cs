using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace Demo
{
    public class CameraLogicPart : LogicPart
    {
        public CameraLogicPart()
            : base(
            new CameraWorld(),
            ()=>new SimpleMovementTwoPlayersKeyboardControllerPool())
        {
            Bots["Square"]=()=>new SquareWalkingBot(50);
            Bots["Random"]=()=>new RandomWalkingBot(50);
        }

        public override Settings GetDefaultSettings()
        {
            return new Settings
            {
                TimeLimit = double.PositiveInfinity,
                OperationalTimeLimit = double.PositiveInfinity,
                Controllers = 
                {
                    new ControllerSettings { ControllerId=TwoPlayersId.Left, Name="Square", Type= ControllerType.Bot },
                    new ControllerSettings { ControllerId=TwoPlayersId.Right, Name="Random", Type= ControllerType.Bot }
                }
            };
        }
    }
}
