using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using RoboMovies.Bots;

namespace RoboMovies
{

    public delegate void DemoTestEntry(CvarcClient<FullMapSensorData, MoveAndBuildCommand> client, RMWorld world, IAsserter asserter);

    public class DemoTestBase : DelegatedCvarcTest<FullMapSensorData, MoveAndBuildCommand, RMWorld, RMWorldState>
    {
        public override SettingsProposal GetSettings()
        {
            return new SettingsProposal
            {
                TimeLimit = 10,
                Controllers = new List<ControllerSettings> 
                    {
                        new ControllerSettings  { ControllerId=TwoPlayersId.Left, Name="This", Type= ControllerType.Client},
                        new ControllerSettings  { ControllerId=TwoPlayersId.Right, Name="Stand", Type= ControllerType.Bot}
                    }
            };
        }

        RMWorldState WorldState;

        public override RMWorldState GetWorldState()
        {
            return WorldState;
        }
        public DemoTestBase(DemoTestEntry entry, RMWorldState state)
            : base((client, world, asserter) => { entry(client, world, asserter); })
        {
            WorldState = state;
        }
    }

    public class RMLogicPartHepler : LogicPartHelper
    {
		       
		public override LogicPart Create()
		{
			var rules = RMRules.Current;

            var logicPart = new LogicPart();
            logicPart.CreateWorld = () => new RMWorld();
            logicPart.CreateDefaultSettings = () => new Settings { OperationalTimeLimit = 5, TimeLimit = 90 };
            logicPart.CreateWorldState = stateName => new RMWorldState() { Seed=int.Parse(stateName) };
            logicPart.PredefinedWorldStates.AddRange(Enumerable.Range(0,10).Select(z=>z.ToString()));
            logicPart.WorldStateType = typeof(RMWorldState);

			var actorFactory = ActorFactory.FromRobot(new RMRobot<FullMapSensorData>(), rules);
            logicPart.Actors[TwoPlayersId.Left]=actorFactory;
			logicPart.Actors[TwoPlayersId.Right]=actorFactory;

 	     logicPart.Bots["Stand"] = () => rules.CreateStandingBot();

         logicPart.Tests["MoveForward"] = new DemoTestBase(
             (cl, w, a) =>
             {
                 cl.Act(rules.Move(100));
                 cl.Act(rules.Stand(1)); var sens = cl.Act(rules.Collect());
                 a.IsEqual(sens.CollectedDetailsCount, 1, 0);
             }, 
             new RMWorldState()
             );
            //logicPart.Bots[RoboMoviesBots.Azura.ToString()] = () => new Azura();
			//logicPart.Bots[RoboMoviesBots.Vaermina.ToString()] = () => new Vaermina();
			//logicPart.Bots[RoboMoviesBots.MolagBal.ToString()] = () => new MolagBal();
			//logicPart.Bots[RoboMoviesBots.Sanguine.ToString()] = () => new Sanguine();
			//logicPart.Bots[RoboMoviesBots.None.ToString()] = () => rules.CreateStandingBot();

			return logicPart;
		}
    }
}
