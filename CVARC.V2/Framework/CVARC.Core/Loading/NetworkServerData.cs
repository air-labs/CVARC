using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CVARC.V2
{
    public enum NetworkServerState
    {
        Waiting,
        Ready,
        Fail,
        Unloaded
    }

    public class NetworkServerData
    {
        /// <summary>
        /// The port on which the server is operating
        /// </summary>
        public int Port { get; set; }
        
        
        /// <summary>
        /// True when Listener was set up 
        /// </summary>
        public NetworkServerState ServerState { get; set; }


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

        public IWorldState WorldState { get; set; }

        public IWorld World { get; set; }

        public Action StopServer { get; set; }

   
        public void Close()
        {
            if (ServerState != NetworkServerState.Ready)
                return;
            StopServer();
            ServerState = NetworkServerState.Unloaded;

        }


        public void WaitForServer()
        {
            while (ServerState == NetworkServerState.Waiting) Thread.Sleep(1);
            if (ServerState != NetworkServerState.Ready)
                throw new Exception("Server initialisation have failed");
        }

        public IWorld WaitForWorld()
        {
            while (World == null) Thread.Sleep(1);
            return World;
        }
    }
}
