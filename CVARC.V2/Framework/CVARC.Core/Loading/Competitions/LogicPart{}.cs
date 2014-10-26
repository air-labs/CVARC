using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class LogicPart<TWorld,TKeyboardControllerPool,TActor,TCommandPreprocessor,TNetworkController> : LogicPart
        where TWorld : IWorld,new()
        where TKeyboardControllerPool : IKeyboardControllerPool, new()
        where TActor : IActor, new()
        where TCommandPreprocessor : ICommandPreprocessor, new()
        where TNetworkController : INetworkController, new()
    {
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

        Func<Settings> defaultSettingsFactory;

        public override Settings GetDefaultSettings()
        {
            return defaultSettingsFactory();
        }

        string[] controllersId;

        public override IEnumerable<string> ControllersId
        {
            get { return controllersId; } 
        }

        public LogicPart(IEnumerable<string> controllersId, Func<Settings> defaultSettingsFactory)
        {
            this.controllersId = controllersId.ToArray();
            this.defaultSettingsFactory = defaultSettingsFactory;
        }

    }
}
