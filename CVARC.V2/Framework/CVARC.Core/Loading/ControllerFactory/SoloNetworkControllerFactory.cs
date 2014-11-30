using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class SoloNetworkControllerFactory : ControllerFactory
    {
        IMessagingClient client;
        bool controllerIsGiven;
        INetworkController controller;
        public SoloNetworkControllerFactory(IMessagingClient client)
        {
            this.client = client;
        }

        public override void Initialize(IWorld world)
        {
            base.Initialize(world);
            controller = world.Competitions.Logic.CreateNetworkController();
            controller.InitializeClient(client);
            
        }

        override public IController Create(string controllerId)
        {
            if (GetSettings(controllerId).Type == ControllerType.Bot)
                return CreateBot(controllerId);
            if (controllerIsGiven)
                throw new Exception("Only one network controller can be assigned in this mode");
            controllerIsGiven = true;
            return controller;
        }
    }
}
