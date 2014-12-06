using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace Demo.Collision
{
    public partial class CollisionLogicPartHelper : LogicPartHelper
    {


        public override LogicPart Create()
        {
            var data = MovementLogicPartHelper.CreateWorldFactory();
            var rules = data.Item1;
            var logicPart = data.Item2;

            logicPart.Bots["Stand"] = () => new Bot<MoveAndGripCommand>(z => rules.Stand(1));
            logicPart.Bots["Grip"] = () => new Bot<MoveAndGripCommand>(z => z == 0 ? rules.Grip() : rules.Stand(1));

            logicPart.Actors[TwoPlayersId.Left] = ActorFactory.FromRobot(new InteractionRobot(), rules);
            logicPart.Actors[TwoPlayersId.Right] = ActorFactory.FromRobot(new InteractionRobot(), rules);

            LoadTests(logicPart, rules);
            return logicPart;
        }
    }
}