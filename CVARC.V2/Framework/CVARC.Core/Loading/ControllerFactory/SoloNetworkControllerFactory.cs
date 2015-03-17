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
        public SoloNetworkControllerFactory(IMessagingClient client)
        {
            this.client = client;
        }


        override public IController Create(string controllerId, IActor actor)
        {
            if (GetSettings(controllerId).Type == ControllerType.Bot)
                return CreateBot(controllerId);
            if (controllerIsGiven)
                throw new Exception("Only one network controller can be assigned in this mode");
            controllerIsGiven = true;
            var controller = World.Competitions.Logic.Actors[controllerId].CreateNetworkController();
            controller.InitializeClient(client);
            return controller;
        }
    }
}
