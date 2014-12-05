using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class LogicPart<TWorld,TActor,TCommand,TCommandPreprocessor,TWorldState,TRules> : LogicPart
        where TWorld : IWorld,new()
        where TActor : IActor, new()
        where TCommandPreprocessor : ICommandPreprocessor, new()
        where TWorldState : IWorldState, new()
        where TRules : IRules, new()
        where TCommand: ICommand, new()
    {

        public LogicPart(IEnumerable<string> controllersId, IEnumerable<string> predefinedStateNames=null)
        {
            this.controllersId = controllersId.ToArray();
            if (predefinedStateNames == null)
                PredefinedStatesNames.Add("Empty");
            else
                PredefinedStatesNames.AddRange(predefinedStateNames);
        }

        public override IActor CreateActor(string controllerName)
        {
            return new TActor();
        }




        public override IWorld CreateWorld()
        {
            return new TWorld();
        }

        string[] controllersId;

        public override IEnumerable<string> ControllersId
        {
            get { return controllersId; } 
        }


        public override IWorldState CreatePredefinedState(string state)
        {
            return new TWorldState();
        }

        public override Settings GetDefaultSettings()
        {
            return new Settings { OperationalTimeLimit = 1, TimeLimit = 10 };
        }

        public override Type GetWorldStateType()
        {
            return typeof(TWorldState);
        }

        public override IRules CreateRulesFor(string controllerName)
        {
            return new TRules();
        }


        public override INetworkController CreateNetworkControllerFor(string controllerId)
        {
            return new NetworkController<TCommand>();
        }
        public override ICommandPreprocessor CreateCommandPreprocessorFor(string controllerName)
        {
            return new TCommandPreprocessor();
        }

        public override IKeyboardController CreateKeyboardControllerFor(string controllerName)
        {
            return new KeyboardController<TCommand>();
        }
    }
}
