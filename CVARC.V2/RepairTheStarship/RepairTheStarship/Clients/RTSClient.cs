using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace RepairTheStarship
{

    public abstract class RTSClient<TSensorData> : CvarcClient<TSensorData, SimpleMovementCommand>
    {
        public abstract string LevelName { get; }

        public TSensorData Configurate(bool isOnLeftSide, RepairTheStarshipBots bot)
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
            return Configurate(configuration);
        }

        public TSensorData Move(double distance)
        {
            return Act(RTSWorld.StaticCommandHelper.Move(distance));
        }

        public TSensorData Rotate(double angleInGrad)
        {
            return Act(RTSWorld.StaticCommandHelper.Rotate(Angle.FromGrad(angleInGrad)));
        }

        public TSensorData Perform(RTSAction action)
        {
            return Act(RTSWorld.StaticCommandHelper.ActionCommand(action.ToString()));
        }

        public void Exit()
        {
            Act(RTSWorld.StaticCommandHelper.ExitCommand());
        }

    }

}
