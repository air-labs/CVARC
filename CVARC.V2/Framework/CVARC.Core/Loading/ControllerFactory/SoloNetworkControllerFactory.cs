using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class SoloNetworkControllerFactory : IControllerFactory
    {
        IMessagingClient client;
        bool controllerIsGiven;
        public SoloNetworkControllerFactory(IMessagingClient client)
        {
            this.client = client;
        }


        public IController Create(ControllerRequest request)
        {
            if (controllerIsGiven)
                    throw new Exception("Only one network controller can be assigned in this mode");

            if (request.ControllerSettings.Type == ControllerType.Bot)
                return request.CreateBot();
            
            var controller = request.Competitions.Logic.CreateNetworkController();
            controller.InitializeClient(client);
            controllerIsGiven = true;
            return controller;
        }
    }
}
