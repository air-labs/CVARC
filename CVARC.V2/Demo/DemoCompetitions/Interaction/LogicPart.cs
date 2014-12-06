using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;

namespace Demo
{
    public partial class InteractionLogicPartHelper : LogicPartHelper
    {



        public override LogicPart Create()
        {
            var data = MovementLogicPartHelper.CreateWorldFactory();
            var rules = data.Item1;
            var logicPart = data.Item2;

            logicPart.Actors[TwoPlayersId.Left] = ActorFactory.FromRobot(new InteractionRobot(), rules);

            LoadTests(logicPart, rules);

            return logicPart;
        }
    }
}
