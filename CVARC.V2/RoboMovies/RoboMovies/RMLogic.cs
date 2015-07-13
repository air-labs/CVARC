using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using RoboMovies.Bots;

namespace RoboMovies
{
    public class RMLogicPartHepler<TSensorsData> : LogicPartHelper
		where TSensorsData : new()
    {
		       
		public override LogicPart Create()
		{
			var rules = RMRules.Current;

            var logicPart = new LogicPart();
            logicPart.CreateWorld = () => new RMWorld();
            logicPart.CreateDefaultSettings = () => new Settings { OperationalTimeLimit = 5, TimeLimit = 90 };
            logicPart.CreateWorldState = stateName => new RTSWorldState() { Seed=int.Parse(stateName) };
            logicPart.PredefinedWorldStates.AddRange(Enumerable.Range(0,10).Select(z=>z.ToString()));
            logicPart.WorldStateType = typeof(RTSWorldState);

			var actorFactory = ActorFactory.FromRobot(new RMRobot<TSensorsData>(), rules);
            logicPart.Actors[TwoPlayersId.Left]=actorFactory;
			logicPart.Actors[TwoPlayersId.Right]=actorFactory;

 	
            //logicPart.Bots[RoboMoviesBots.Azura.ToString()] = () => new Azura();
			//logicPart.Bots[RoboMoviesBots.Vaermina.ToString()] = () => new Vaermina();
			//logicPart.Bots[RoboMoviesBots.MolagBal.ToString()] = () => new MolagBal();
			//logicPart.Bots[RoboMoviesBots.Sanguine.ToString()] = () => new Sanguine();
			//logicPart.Bots[RoboMoviesBots.None.ToString()] = () => rules.CreateStandingBot();

			return logicPart;
		}
    }
}
