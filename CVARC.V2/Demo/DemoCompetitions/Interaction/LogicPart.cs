using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace Demo
{
    public class InteractionLogicPart :  LogicPart<
                                                           MovementWorld,
                                                           SimpleMovementTwoPlayersKeyboardControllerPool,
                                                           MovementRobot,
                                                           SimpleMovementPreprocessor,
                                                           NetworkController<SimpleMovementCommand>,
                                                        MovementWorldState
                                                    >
    {
        public InteractionLogicPart()
            : base(TwoPlayersId.Ids)
        {
            Bots["Bot"]=()=>new MovingForwardBot();
        }

      
    }
}
