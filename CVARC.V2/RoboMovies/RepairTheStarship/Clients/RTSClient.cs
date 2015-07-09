using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2;

namespace RepairTheStarship
{

    public abstract class RTSClient<TSensorData> : CvarcClient<TSensorData, MoveAndGripCommand>
        where TSensorData : class
    {
        public abstract string LevelName { get; }

        public TSensorData Configurate(int port, bool isOnLeftSide, RepairTheStarshipBots bot = RepairTheStarshipBots.None, int seed = 0)
        {
            var configuration = new ConfigurationProposal();
            configuration.LoadingData.AssemblyName = "RepairTheStarship";
            configuration.LoadingData.Level = LevelName;
            configuration.SettingsProposal.Controllers = new List<ControllerSettings>();
            configuration.SettingsProposal.Controllers.Add(new ControllerSettings
            {
                ControllerId = isOnLeftSide ? TwoPlayersId.Left : TwoPlayersId.Right,
                Name = "This",
                Type = ControllerType.Client
            });
            configuration.SettingsProposal.Controllers.Add(new ControllerSettings
            {
                ControllerId = isOnLeftSide ? TwoPlayersId.Right : TwoPlayersId.Left,
                Name = bot.ToString(),
                Type = ControllerType.Bot
            });


            return Configurate(port, configuration, new RTSWorldState { Seed = seed } );
        }

        public TSensorData Move(double distance)
        {
            return Act(RTSRules.Current.Move(distance));
        }

        public TSensorData Rotate(double angleInGrad)
        {
            return Act(RTSRules.Current.Rotate(Angle.FromGrad(angleInGrad)));
        }

        public TSensorData Grip()
        {
            return Act(RTSRules.Current.Grip());
        }

		public TSensorData Release()
		{
			return Act(RTSRules.Current.Release());
		}

        public void Stand(double time)
        {
            Act(RTSRules.Current.Stand(time));
        }

    }

}
