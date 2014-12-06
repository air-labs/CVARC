﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab;
using CVARC.V2;

namespace Demo
{
    public partial class MovementLogicPartHelper : LogicPartHelper
    {

        public static Tuple<MoveAndGripRules,LogicPart> CreateWorldFactory()
        {
            var rules = new MoveAndGripRules();

            var logicPart = new LogicPart();
            logicPart.CreateWorld = () => new MovementWorld();
            logicPart.CreateDefaultSettings = () => new Settings { OperationalTimeLimit = 1, TimeLimit = 10 };
            logicPart.CreateWorldState = stateName => new MovementWorldState();
            logicPart.PredefinedWorldStates.Add("Empty");
            logicPart.WorldStateType = typeof(MovementWorldState);

            return new Tuple<MoveAndGripRules, LogicPart>(rules, logicPart);
        }

        public override LogicPart Create()
        {
            var data = CreateWorldFactory();
            var rules = data.Item1;
            var logicPart = data.Item2;

            var actorFactory = ActorFactory.FromRobot(new MoveAndGripRobot<IActorManager, MovementWorld, SensorsData>(), rules);
            logicPart.Actors[TwoPlayersId.Left] = actorFactory;
            
            logicPart.Bots["Stand"] = () => rules.CreateStandingBot();
            logicPart.Bots["Square"] = () => rules.CreateSquareWalkingBot(50);
            logicPart.Bots["Random"] = () => rules.CreateRandomWalkingBot(50);

            LoadTests(logicPart, rules);

            return logicPart;
        }
    }
}
