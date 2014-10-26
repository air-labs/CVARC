using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public abstract class LogicPart
    {

        public readonly Dictionary<string, Func<IController>> Bots = new Dictionary<string, Func<IController>>();

        public abstract INetworkController CreateNetworkController();

        public abstract Settings GetDefaultSettings();

        public abstract IWorld CreateWorld();

        public abstract IKeyboardControllerPool CreateKeyboardControllerPool();

        public abstract IEnumerable<string> ControllersId { get; }

        public abstract IActor CreateActor(string controllerName);

        public abstract ICommandPreprocessor CreateCommandPreprocessor(string controllerName);
    }
}
