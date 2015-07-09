using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using RepairTheStarship.Bots;

namespace RepairTheStarship
{
    public class RTSLogicPartHepler<TSensorsData> : LogicPartHelper
		where TSensorsData : new()
    {
		       
		public override LogicPart Create()
		{
			var rules = RTSRules.Current;

            var logicPart = new LogicPart();
            logicPart.CreateWorld = () => new RTSWorld();
            logicPart.CreateDefaultSettings = () => new Settings { OperationalTimeLimit = 5, TimeLimit = 90 };
            logicPart.CreateWorldState = stateName => new RTSWorldState() { Seed=int.Parse(stateName) };
            logicPart.PredefinedWorldStates.AddRange(Enumerable.Range(0,10).Select(z=>z.ToString()));
            logicPart.WorldStateType = typeof(RTSWorldState);

			var actorFactory = ActorFactory.FromRobot(new RTSRobot<TSensorsData>(), rules);
            logicPart.Actors[TwoPlayersId.Left]=actorFactory;
			logicPart.Actors[TwoPlayersId.Right]=actorFactory;

 	
            logicPart.Bots[RepairTheStarshipBots.Azura.ToString()] = () => new Azura();
			logicPart.Bots[RepairTheStarshipBots.Vaermina.ToString()] = () => new Vaermina();
			logicPart.Bots[RepairTheStarshipBots.MolagBal.ToString()] = () => new MolagBal();
			logicPart.Bots[RepairTheStarshipBots.Sanguine.ToString()] = () => new Sanguine();
			logicPart.Bots[RepairTheStarshipBots.None.ToString()] = () => rules.CreateStandingBot();

			return logicPart;
		}
    }
}
