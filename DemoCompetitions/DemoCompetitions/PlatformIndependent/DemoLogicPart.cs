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
            ()=>new SimpleMovementTwoPlayersKeyboardControllerPool(),
            seed=>new SceneSettings()
            )
        {
            Bots["Square"]=()=>new SquareWalkingBot(50);
            Bots["Random"]=()=>new RandomWalkingBot(50);
        }
    }
}
