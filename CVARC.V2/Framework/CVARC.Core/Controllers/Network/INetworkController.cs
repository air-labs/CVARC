using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public  interface INetworkController : IController
    {
        double OperationalTimeLimit { get; set; }
        void InitializeClient(IMessagingClient client);
    }
}
