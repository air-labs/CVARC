using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab;
using CVARC.V2;

namespace Demo
{
    public partial class DWMLogicPartHelper : LogicPartHelper
    {

		public static Tuple<DWMRules, LogicPart> CreateWorldFactory()
        {
            var rules = new DWMRules();

            var logicPart = new LogicPart();
            logicPart.CreateWorld = () => new DWMWorld();
            logicPart.CreateDefaultSettings = () => new Settings { OperationalTimeLimit = 1, TimeLimit = 15 };
			logicPart.CreateWorldState = stateName => new DWMWorldState(0, 0);
            logicPart.PredefinedWorldStates.Add("Empty");
			logicPart.WorldStateType = typeof(DWMWorldState);


			


            return new Tuple<DWMRules, LogicPart>(rules, logicPart);
        }

        public override LogicPart Create()
        {
            var data = CreateWorldFactory();
            var rules = data.Item1;
            var logicPart = data.Item2;

            var actorFactory = ActorFactory.FromRobot(new DWMRobot(), rules);
			actorFactory.CreateCommandFilterSet = 
				()=> new CommandFilterSet {
						Filters = { new DWMDistortionCommandFilter() }
				};

            logicPart.Actors[TwoPlayersId.Left] = actorFactory;
           


         	LoadDWMTests(logicPart, rules);
			LoadEncodersTests(logicPart, rules);
			LoadGAXTests(logicPart, rules);

            return logicPart;
        }
    }
}
