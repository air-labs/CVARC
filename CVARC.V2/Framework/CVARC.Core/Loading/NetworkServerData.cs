using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CVARC.V2
{
    public class NetworkServerData
    {
        /// <summary>
        /// The port on which the server is operating
        /// </summary>
        public int Port { get; set; }
        
        
        /// <summary>
        /// True when Listener is up
        /// </summary>
        public bool ServerLoaded { get; set; }

        /// <summary>
        /// The connection on server that waits for commands
        /// </summary>
        public IMessagingClient ClientOnServerSide { get; set; }


        /// <summary>
        /// Data about which competitions to run
        /// </summary>
        public LoadingData LoadingData { get; set; }

        /// <summary>
        /// The resulting settings
        /// </summary>
        public Settings Settings { get; set; }

        public IWorld World { get; set; }

        public Action StopServer { get; set; }

   
        public void Close()
        {
            if (!ServerLoaded) return;
            StopServer();
            ServerLoaded = false;
        }


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
