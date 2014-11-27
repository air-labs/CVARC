using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public  interface INetworkController : IController
    {
        void InitializeClient(IMessagingClient client);
    }
}
