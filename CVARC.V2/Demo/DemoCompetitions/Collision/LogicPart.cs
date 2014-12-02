using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace Demo.Collision
{
    public partial class CollisionLogicPart : LogicPart<
                                                       CollisionWorld,
                                                       SimpleMovementTwoPlayersKeyboardControllerPool,
                                                       CollisionRobot,
                                                       SimpleMovementPreprocessor,
                                                       NetworkController<SimpleMovementCommand>,
                                                       MovementWorldState

                                                >
    {
        public CollisionLogicPart()
            : base(TwoPlayersId.Ids)
        {
            Bots["Forward"] = () => new PushingBot(false, true);
            Bots["Backward"] = () => new PushingBot(false, false);
            Bots["Detail"] = () => new PushingBot(true, true);
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
                    new ControllerSettings { ControllerId=TwoPlayersId.Left, Name="Bot1", Type= ControllerType.Bot }
                }
            };
        }
    }

}
