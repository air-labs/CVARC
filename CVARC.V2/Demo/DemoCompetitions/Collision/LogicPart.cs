using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace Demo.Collision
{
    public class CollisionLogicPart : LogicPart<
                                                       CollisionWorld,
                                                       SimpleMovementTwoPlayersKeyboardControllerPool,
                                                       CollisionRobot,
                                                       SimpleMovementPreprocessor,
                                                       NetworkController<SimpleMovementCommand>
                                                >
    {
        public CollisionLogicPart()
            : base(TwoPlayersId.Ids)
        {
            Bots["Forward"] = () => new PushingBot(false, true);
            Bots["Backward"] = () => new PushingBot(false, false);
            Bots["Detail"] = () => new PushingBot(true, true);
        }
    }

}
