using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace Demo
{
    public class MovementLogicPart : LogicPart
    {
        public MovementLogicPart()
            : base(
            new MovementWorld(),
            ()=>new SimpleMovementTwoPlayersKeyboardControllerPool())
        {
            Bots["Square"]=()=>new SquareWalkingBot(50);
            Bots["Random"]=()=>new RandomWalkingBot(50);
        }

        public override Settings GetDefaultSettings()
        {
            return new Settings { OperationalTimeLimit = 5, TimeLimit = double.PositiveInfinity };
        }
    }
}
