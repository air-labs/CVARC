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
            return new Settings { OperationalTimeLimit = 5, TimeLimit = double.PositiveInfinity };
        }
    }
}
