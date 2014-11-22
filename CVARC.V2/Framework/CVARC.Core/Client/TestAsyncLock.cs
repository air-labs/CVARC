using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CVARC.V2
{
    public class TestAsyncLock
    {
        public bool ServerLoaded { get; set; }

        public IWorld World { get; set; }

        public TcpListener Listener { get; set; }

        public TcpClient Client { get; set; }

        public void WaitForServer()
        {
            while (!ServerLoaded) Thread.Sleep(1);
        }

        public IWorld WaitForWorld()
        {
            while (World == null) Thread.Sleep(1);
            return World;
        }
    }
}
