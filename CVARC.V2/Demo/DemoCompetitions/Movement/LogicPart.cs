using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace Demo
{
    public class MovementLogicPart :  LogicPart<
                                                           MovementWorld,
                                                           SimpleMovementTwoPlayersKeyboardControllerPool,
                                                           MovementRobot,
                                                           SimpleMovementPreprocessor,
                                                           NetworkController<SimpleMovementCommand> 
                                                >
    {
        public MovementLogicPart()
            : base(new[] { ControllerId }, GetDefaultSettings)
        {
            Bots["Square"]=()=>new SquareWalkingBot(50);
            Bots["Random"]=()=>new RandomWalkingBot(50);

            LoadTests();
        }

        void LoadTests()
        {
            Tests["Forward"] = new MovementTestBase((client, world, asserter) =>
                {
                    client.Act(SimpleMovementCommand.Move(10, 1));
                    var location = world.Engine.GetAbsoluteLocation(world.Actors.First().ObjectId);
                    asserter.IsEqual(10, location.X, 1e-3);
                });
        }

        public const string ControllerId = "Robot";

        static Settings GetDefaultSettings()
        {
            return new Settings
            {
                TimeLimit = double.PositiveInfinity,
                OperationalTimeLimit = 1,
                Controllers = 
                {
                    new ControllerSettings { ControllerId=ControllerId, Name="Square", Type= ControllerType.Bot },
                }
            };
        }
    }
}
