using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace Demo
{

    public class CameraLogicPart : LogicPart <
                                                    MovementWorld,
                                                    SimpleMovementTwoPlayersKeyboardControllerPool,
                                                    CameraRobot,
                                                    SimpleMovementPreprocessor,
                                                    NetworkController<SimpleMovementCommand>,
                                                        MovementWorldState
                                             >
    {
        public CameraLogicPart()
            : base(TwoPlayersId.Ids)
        {
            Bots["Square"]=()=>new SquareWalkingBot(50);
            Bots["Random"]=()=>new RandomWalkingBot(50);
        }

       
       
    }
}
