using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public abstract class LogicPart
    {
        public readonly Dictionary<string, ICvarcTest> Tests = new Dictionary<string, ICvarcTest>();

        public readonly Dictionary<string, Func<IController>> Bots = new Dictionary<string, Func<IController>>();

        public abstract Settings GetDefaultSettings();

        public abstract IWorld CreateWorld();

        public abstract IEnumerable<string> ControllersId { get; }

        public abstract IActor CreateActor(string controllerName);



        public abstract IWorldState CreatePredefinedState(string state);

        public readonly List<string> PredefinedStatesNames = new List<string>();

        public abstract Type GetWorldStateType();

        public abstract IRules CreateRulesFor(string controllerName);
        public abstract ICommandPreprocessor CreateCommandPreprocessorFor(string controllerName);
        public abstract IKeyboardController CreateKeyboardControllerFor(string controllerName);
        public abstract INetworkController CreateNetworkControllerFor(string controllerName);
    }
}
