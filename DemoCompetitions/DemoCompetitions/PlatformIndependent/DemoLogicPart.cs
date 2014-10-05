using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace DemoCompetitions
{
    public class DemoLogicPart : LogicPart
    {
        public DemoLogicPart() : base(
            new DemoWorld(),
            keyboard=>new SimpleMovementTwoPlayersKeyboardControllerPool(keyboard),
            seed=>new SceneSettings()
            )
        {
            Bots["Square"]=controllerId=>new SquareWalkingBot(controllerId,50);
            Bots["Random"]=controllerId=>new RandomWalkingBot(controllerId,50);

        }
    }
}
