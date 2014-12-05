using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public abstract class LogicPart<TWorld,TKeyboardControllerPool,TActor,TCommandPreprocessor,TNetworkController,TWorldState,TRules> : LogicPart
        where TWorld : IWorld,new()
        where TKeyboardControllerPool : IKeyboardControllerPool, new()
        where TActor : IActor, new()
        where TCommandPreprocessor : ICommandPreprocessor, new()
        where TNetworkController : INetworkController, new()
        where TWorldState : IWorldState, new()
        where TRules : IRules, new()
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

        public override ICommandPreprocessor CreateCommandPreprocessor(string controllerName)
        {
            return new TCommandPreprocessor();
        }

        public override IKeyboardControllerPool CreateKeyboardControllerPool()
        {
            return new TKeyboardControllerPool();
        }

        public override INetworkController CreateNetworkController()
        {
            return new TNetworkController();
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

        public override IRules CreateRulesForController(string controllerName)
        {
            return new TRules();
        }
    }
}
