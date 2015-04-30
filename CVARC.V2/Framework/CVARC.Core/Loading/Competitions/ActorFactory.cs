using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CVARC.V2
{
    public class ActorFactory
    {
        public Func<IActor> CreateActor { get; set; }
        public Func<IRules> CreateRules { get; set; }
        public Func<CommandFilterSet> CreateCommandFilterSet { get; set; }
        public Func<INetworkController> CreateNetworkController { get; set; }
        public Func<IKeyboardController> CreateKeyboardController { get; set; }

        public static ActorFactory FromRobot<TActorManager,TWorld,TSensorsData,TCommand,TRules>
                                            (Robot<TActorManager,TWorld,TSensorsData,TCommand,TRules> etalonRobot, IRules rules)
            where TActorManager : IActorManager
            where TWorld : IWorld
            where TSensorsData : new()
            where TCommand : ICommand
            where TRules : IRules
        {
            var factory = new ActorFactory();
            factory.CreateNetworkController = () => new NetworkController<TCommand>();
            factory.CreateKeyboardController = () => new KeyboardController<TCommand>();
            factory.CreateCommandFilterSet = () => new CommandFilterSet();
            factory.CreateRules = () => rules;

            factory.CreateActor =
                Expression.Lambda<Func<IActor>>(
                    Expression.Convert(
                        Expression.New(etalonRobot.GetType()),
                        typeof(IActor))).Compile();

            return factory;
        }
    }
}
