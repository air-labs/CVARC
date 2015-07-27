using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab;
using CVARC.V2;

namespace Demo
{
    public partial class DemoLogicPartHelper : LogicPartHelper
    {

        public static Tuple<DemoRules,LogicPart> CreateWorldFactory()
        {
            var rules = new DemoRules();

            var logicPart = new LogicPart();
            logicPart.CreateWorld = () => new DemoWorld();
            logicPart.CreateDefaultSettings = () => new Settings { OperationalTimeLimit = 1, TimeLimit = 15 };
            logicPart.CreateWorldState = stateName => new DemoWorldState();
            logicPart.PredefinedWorldStates.Add("Empty");
            logicPart.WorldStateType = typeof(DemoWorldState);


            return new Tuple<DemoRules, LogicPart>(rules, logicPart);
        }

        public override LogicPart Create()
        {
            var data = CreateWorldFactory();
            var rules = data.Item1;
            var logicPart = data.Item2;

            var actorFactory = ActorFactory.FromRobot(new DemoRobot(), rules);
            logicPart.Actors[TwoPlayersId.Left] = actorFactory;
            logicPart.Actors[TwoPlayersId.Right] = actorFactory;            
            logicPart.Bots["Stand"] = () => rules.CreateStandingBot();
            logicPart.Bots["Square"] = () => rules.CreateSquareWalkingBot(50);
            logicPart.Bots["Random"] = () => rules.CreateRandomWalkingBot(50);


            LoadMovementTests(logicPart, rules);
			LoadInteractionTests(logicPart, rules);
            LoadGrippingTests(logicPart, rules);
            LoadCollisionTests(logicPart, rules);
		
            return logicPart;
        }
    }
}
