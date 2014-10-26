using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CVARC.V2
{
    public class DebugRunMode : IRunMode
    {
        int givenControllers = 0;
        INetworkController controller;
        ConfigurationProposal proposal;
        TcpClient client;
        CvarcTcpClient grobo;
        public DebugRunMode(int portNumber)
        {
            var tcpServer = new System.Net.Sockets.TcpListener(portNumber);
            tcpServer.Start();
            client = tcpServer.AcceptTcpClient();
            grobo = new CvarcTcpClient(client);
            proposal = grobo.ReadObject<ConfigurationProposal>();
        }

        public ConfigurationProposal GetConfigurationProposal()
        {
            return proposal;
        }

        public void Initialize(IWorld world, Configuration configuration, Competitions competitions)
        {
            this.Configuration = configuration; 
            this.Competitions = competitions;
            controller = Competitions.Logic.CreateNetworkController();
            controller.InitializeClient(grobo);
            controller.OperationalTimeLimit = configuration.Settings.OperationalTimeLimit;
            world.Exit += World_Exit;
        }

        void World_Exit()
        {
            client.Close();
        }

        public IController GetController(string controllerId)
        {
            var record = this.GetControllerConfigFor(controllerId);
            if (record.Type == ControllerType.Bot)
                return this.GetBotFor(record);
            if (givenControllers != 0)
                throw new Exception("Only one network controller can be assigned in this mode");
            givenControllers++;
            return controller;
        }




        public Configuration Configuration
        {
            get;
            private set; 
        }

        public Competitions Competitions
        {
            get;
            private set; 
        }
    }
}
