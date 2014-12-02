using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace Demo
{
    public partial class InteractionLogicPart :  LogicPart<
                                                           MovementWorld,
                                                           SimpleMovementTwoPlayersKeyboardControllerPool,
                                                           InteractionRobot<InteractionSensorData>,
                                                           SimpleMovementPreprocessor,
                                                           NetworkController<SimpleMovementCommand>,
                                                           MovementWorldState
                                                    >
    {
        public InteractionLogicPart()
            : base(new [] {"Left"})
        {
            Bots["Bot"]=()=>new MovingForwardBot();
            LoadTests();
        }
        static Settings GetDefaultSettings()
        {
            return new Settings
            {
                TimeLimit = double.PositiveInfinity,
                OperationalTimeLimit = double.PositiveInfinity,
                Controllers = 
                {
                    new ControllerSettings { ControllerId=TwoPlayersId.Left, Name="Bot", Type= ControllerType.Bot }
                }
            };
        }
      
    }
}
